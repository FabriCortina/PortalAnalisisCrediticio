using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.Auth;
using PortalAnalisisCrediticio.Shared.DTOs.Usuario;

namespace PortalAnalisisCrediticio.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private const int CACHE_DURATION_MINUTES = 15;
        private const int MAX_LOGIN_ATTEMPTS = 5;
        private const int LOGIN_ATTEMPT_WINDOW_MINUTES = 15;
        private const int PASSWORD_RESET_TOKEN_EXPIRATION_HOURS = 24;

        public AuthService(
            ApplicationDbContext context,
            ILogger<AuthService> logger,
            IMemoryCache cache,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            try
            {
                _logger.LogInformation($"Intento de inicio de sesión para usuario: {loginDto.Email}");

                var cacheKey = $"login_attempts_{loginDto.Email}";
                var loginAttempts = await GetLoginAttemptsAsync(loginDto.Email);

                if (loginAttempts >= MAX_LOGIN_ATTEMPTS)
                {
                    _logger.LogWarning($"Cuenta bloqueada por exceso de intentos: {loginDto.Email}");
                    throw new Exception("Cuenta bloqueada por exceso de intentos. Intente más tarde.");
                }

                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .ThenInclude(r => r.Permisos)
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

                if (usuario == null || !BC.Verify(loginDto.Password, usuario.PasswordHash))
                {
                    await IncrementLoginAttemptsAsync(loginDto.Email);
                    _logger.LogWarning($"Credenciales inválidas para usuario: {loginDto.Email}");
                    throw new Exception("Credenciales inválidas");
                }

                if (!usuario.EmailConfirmado)
                {
                    _logger.LogWarning($"Email no confirmado para usuario: {loginDto.Email}");
                    throw new Exception("Por favor confirme su email antes de iniciar sesión");
                }

                if (!usuario.Activo)
                {
                    _logger.LogWarning($"Cuenta inactiva para usuario: {loginDto.Email}");
                    throw new Exception("Su cuenta está inactiva. Contacte al administrador");
                }

                var token = GenerateJwtToken(usuario);
                var permisos = await GetUserPermissionsAsync(usuario.Id);

                await ResetLoginAttemptsAsync(loginDto.Email);
                _logger.LogInformation($"Inicio de sesión exitoso para usuario: {loginDto.Email}");

                return new LoginResponseDTO
                {
                    Token = token,
                    Usuario = new UsuarioDTO
                    {
                        Id = usuario.Id,
                        Email = usuario.Email,
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        Rol = usuario.Rol.Nombre
                    },
                    Permisos = permisos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en inicio de sesión para usuario: {loginDto.Email}");
                throw;
            }
        }

        public async Task<UsuarioDTO> RegistroAsync(RegistroDTO registroDto)
        {
            try
            {
                _logger.LogInformation($"Iniciando registro de usuario: {registroDto.Email}");

                if (await _context.Usuarios.AnyAsync(u => u.Email == registroDto.Email))
                {
                    _logger.LogWarning($"Email ya registrado: {registroDto.Email}");
                    throw new Exception("El email ya está registrado");
                }

                if (!registroDto.AceptaTerminos)
                {
                    _logger.LogWarning($"Términos no aceptados para usuario: {registroDto.Email}");
                    throw new Exception("Debe aceptar los términos y condiciones");
                }

                var rolCliente = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Cliente");
                if (rolCliente == null)
                {
                    _logger.LogError("Rol Cliente no encontrado en la base de datos");
                    throw new Exception("Error en la configuración del sistema");
                }

                var usuario = new Usuario
                {
                    Email = registroDto.Email,
                    PasswordHash = BC.HashPassword(registroDto.Password),
                    Nombre = registroDto.Nombre,
                    Apellido = registroDto.Apellido,
                    RolId = rolCliente.Id,
                    EmailConfirmado = false,
                    Activo = true,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                var token = GenerateEmailConfirmationToken(usuario);
                await _emailService.EnviarEmailConfirmacionAsync(usuario.Email, token);

                _logger.LogInformation($"Usuario registrado exitosamente: {registroDto.Email}");

                return new UsuarioDTO
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Rol = rolCliente.Nombre
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en registro de usuario: {registroDto.Email}");
                throw;
            }
        }

        public async Task<bool> CambiarPasswordAsync(CambiarPasswordDTO cambiarPasswordDto)
        {
            try
            {
                _logger.LogInformation($"Iniciando cambio de contraseña para usuario: {cambiarPasswordDto.Email}");

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == cambiarPasswordDto.Email);

                if (usuario == null)
                {
                    _logger.LogWarning($"Usuario no encontrado: {cambiarPasswordDto.Email}");
                    throw new Exception("Usuario no encontrado");
                }

                if (!BC.Verify(cambiarPasswordDto.PasswordActual, usuario.PasswordHash))
                {
                    _logger.LogWarning($"Contraseña actual incorrecta para usuario: {cambiarPasswordDto.Email}");
                    throw new Exception("Contraseña actual incorrecta");
                }

                usuario.PasswordHash = BC.HashPassword(cambiarPasswordDto.PasswordNueva);
                usuario.FechaActualizacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await _emailService.EnviarEmailNotificacionAsync(
                    usuario.Email,
                    "Cambio de Contraseña",
                    "Su contraseña ha sido cambiada exitosamente"
                );

                _logger.LogInformation($"Contraseña cambiada exitosamente para usuario: {cambiarPasswordDto.Email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cambiar contraseña para usuario: {cambiarPasswordDto.Email}");
                throw;
            }
        }

        public async Task<bool> RecuperarPasswordAsync(string email)
        {
            try
            {
                _logger.LogInformation($"Iniciando recuperación de contraseña para: {email}");

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (usuario == null)
                {
                    _logger.LogWarning($"Usuario no encontrado para recuperación: {email}");
                    return false;
                }

                var token = GeneratePasswordResetToken(usuario);
                var tokenExpiracion = DateTime.UtcNow.AddHours(PASSWORD_RESET_TOKEN_EXPIRATION_HOURS);

                usuario.TokenRecuperacion = token;
                usuario.TokenExpiracion = tokenExpiracion;
                await _context.SaveChangesAsync();

                await _emailService.EnviarEmailRecuperacionPasswordAsync(email, token);

                _logger.LogInformation($"Email de recuperación enviado a: {email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en recuperación de contraseña para: {email}");
                throw;
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO resetPasswordDto)
        {
            try
            {
                _logger.LogInformation($"Iniciando reset de contraseña con token: {resetPasswordDto.Token}");

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.TokenRecuperacion == resetPasswordDto.Token);

                if (usuario == null || usuario.TokenExpiracion < DateTime.UtcNow)
                {
                    _logger.LogWarning($"Token inválido o expirado: {resetPasswordDto.Token}");
                    throw new Exception("Token inválido o expirado");
                }

                usuario.PasswordHash = BC.HashPassword(resetPasswordDto.PasswordNueva);
                usuario.TokenRecuperacion = null;
                usuario.TokenExpiracion = null;
                usuario.FechaActualizacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await _emailService.EnviarEmailNotificacionAsync(
                    usuario.Email,
                    "Contraseña Restablecida",
                    "Su contraseña ha sido restablecida exitosamente"
                );

                _logger.LogInformation($"Contraseña restablecida exitosamente para usuario: {usuario.Email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al resetear contraseña con token: {resetPasswordDto.Token}");
                throw;
            }
        }

        public async Task<bool> ConfirmarEmailAsync(string token)
        {
            try
            {
                _logger.LogInformation($"Iniciando confirmación de email con token: {token}");

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.TokenConfirmacion == token);

                if (usuario == null || usuario.TokenExpiracion < DateTime.UtcNow)
                {
                    _logger.LogWarning($"Token de confirmación inválido o expirado: {token}");
                    throw new Exception("Token inválido o expirado");
                }

                usuario.EmailConfirmado = true;
                usuario.TokenConfirmacion = null;
                usuario.TokenExpiracion = null;
                usuario.FechaActualizacion = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Email confirmado exitosamente para usuario: {usuario.Email}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al confirmar email con token: {token}");
                throw;
            }
        }

        private async Task<List<string>> GetUserPermissionsAsync(int usuarioId)
        {
            try
            {
                var cacheKey = $"permisos_usuario_{usuarioId}";
                if (_cache.TryGetValue(cacheKey, out List<string> cachedPermissions))
                {
                    return cachedPermissions;
                }

                var permisos = await _context.Usuarios
                    .Where(u => u.Id == usuarioId)
                    .SelectMany(u => u.Rol.Permisos)
                    .Select(p => p.Nombre)
                    .Distinct()
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, permisos, cacheOptions);
                return permisos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener permisos para usuario: {usuarioId}");
                throw;
            }
        }

        private async Task<int> GetLoginAttemptsAsync(string email)
        {
            var cacheKey = $"login_attempts_{email}";
            return _cache.TryGetValue(cacheKey, out int attempts) ? attempts : 0;
        }

        private async Task IncrementLoginAttemptsAsync(string email)
        {
            var cacheKey = $"login_attempts_{email}";
            var attempts = await GetLoginAttemptsAsync(email);
            
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(LOGIN_ATTEMPT_WINDOW_MINUTES));

            _cache.Set(cacheKey, attempts + 1, cacheOptions);
        }

        private async Task ResetLoginAttemptsAsync(string email)
        {
            var cacheKey = $"login_attempts_{email}";
            _cache.Remove(cacheKey);
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateEmailConfirmationToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GeneratePasswordResetToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(PASSWORD_RESET_TOKEN_EXPIRATION_HOURS),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 