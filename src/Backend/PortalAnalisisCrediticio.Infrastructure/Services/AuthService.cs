using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.Auth;
using System.IdentityModel.Tokens.Jwt;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthService(
        ApplicationDbContext context,
        IConfiguration configuration,
        IEmailService emailService)
    {
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<UsuarioDTO> LoginAsync(LoginDTO loginDto)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Permisos)
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Activo);

        if (usuario == null || !VerifyPasswordHash(loginDto.Password, usuario.PasswordHash))
        {
            await RegistrarAuditoriaAcceso(usuario?.Id, "AccesoDenegado", "Credenciales inválidas");
            return null;
        }

        usuario.UltimoAcceso = DateTime.Now;
        await _context.SaveChangesAsync();

        await RegistrarAuditoriaAcceso(usuario.Id, "Login", "Acceso exitoso");

        return MapToUsuarioDTO(usuario);
    }

    public async Task<UsuarioDTO> RegistroAsync(RegistroDTO registroDto)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == registroDto.Email))
            throw new Exception("El email ya está registrado");

        var usuario = new Usuario
        {
            Nombre = registroDto.Nombre,
            Apellido = registroDto.Apellido,
            Email = registroDto.Email,
            PasswordHash = HashPassword(registroDto.Password),
            Rol = "Vendedor", // Rol por defecto
            AceptoTerminos = registroDto.AceptoTerminos,
            FechaAceptacionTerminos = DateTime.Now,
            AceptoMarketing = registroDto.AceptoMarketing,
            FechaAceptacionMarketing = registroDto.AceptoMarketing ? DateTime.Now : null
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        // Asignar permisos por defecto según el rol
        await AsignarPermisosPorDefecto(usuario.Id, usuario.Rol);

        return MapToUsuarioDTO(usuario);
    }

    public async Task<bool> CambiarPasswordAsync(int usuarioId, string passwordActual, string passwordNuevo)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId);
        if (usuario == null || !VerifyPasswordHash(passwordActual, usuario.PasswordHash))
            return false;

        usuario.PasswordHash = HashPassword(passwordNuevo);
        await _context.SaveChangesAsync();

        await RegistrarAuditoriaAcceso(usuarioId, "CambioPassword", "Cambio de contraseña exitoso");
        return true;
    }

    public async Task<bool> RecuperarPasswordAsync(string email)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        if (usuario == null)
            return false;

        var token = GeneratePasswordResetToken();
        // TODO: Guardar token en base de datos con expiración

        await _emailService.EnviarEmailRecuperacionPasswordAsync(email, token);
        return true;
    }

    public async Task<bool> VerificarPermisoAsync(int usuarioId, string recurso, string accion)
    {
        return await _context.Permisos
            .AnyAsync(p => p.UsuarioId == usuarioId && 
                          p.Recurso == recurso && 
                          p.Accion == accion && 
                          p.Permitido);
    }

    public async Task<List<PermisoDTO>> GetPermisosAsync(int usuarioId)
    {
        var permisos = await _context.Permisos
            .Where(p => p.UsuarioId == usuarioId)
            .Select(p => new PermisoDTO
            {
                Recurso = p.Recurso,
                Accion = p.Accion,
                Permitido = p.Permitido
            })
            .ToListAsync();

        return permisos;
    }

    public async Task<bool> ActualizarPermisosAsync(int usuarioId, List<PermisoDTO> permisos)
    {
        var permisosActuales = await _context.Permisos
            .Where(p => p.UsuarioId == usuarioId)
            .ToListAsync();

        _context.Permisos.RemoveRange(permisosActuales);

        var nuevosPermisos = permisos.Select(p => new Permiso
        {
            UsuarioId = usuarioId,
            Recurso = p.Recurso,
            Accion = p.Accion,
            Permitido = p.Permitido
        });

        _context.Permisos.AddRange(nuevosPermisos);
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task AsignarPermisosPorDefecto(int usuarioId, string rol)
    {
        var permisos = new List<Permiso>();

        switch (rol)
        {
            case "Administrador":
                permisos.AddRange(new[]
                {
                    new Permiso { UsuarioId = usuarioId, Recurso = "Usuarios", Accion = "Ver", Permitido = true },
                    new Permiso { UsuarioId = usuarioId, Recurso = "Usuarios", Accion = "Editar", Permitido = true },
                    new Permiso { UsuarioId = usuarioId, Recurso = "Usuarios", Accion = "Eliminar", Permitido = true },
                    new Permiso { UsuarioId = usuarioId, Recurso = "Informes", Accion = "Ver", Permitido = true },
                    new Permiso { UsuarioId = usuarioId, Recurso = "Informes", Accion = "Generar", Permitido = true },
                    new Permiso { UsuarioId = usuarioId, Recurso = "Clientes", Accion = "Ver", Permitido = true },
                    new Permiso { UsuarioId = usuarioId, Recurso = "Clientes", Accion = "Editar", Permitido = true }
                });
                break;

            case "AnalistaCredito":
                permisos.AddRange(new[]
                {
                    new Permiso { UsuarioId = usuarioId, Recurso = "Informes", Accion = "Ver", Permitido = true },
                    new Permiso { UsuarioId = usuarioId, Recurso = "Informes", Accion = "Generar", Permitido = true },
                    new Permiso { UsuarioId = usuarioId, Recurso = "Clientes", Accion = "Ver", Permitido = true },
                    new Permiso { UsuarioId = usuarioId, Recurso = "Clientes", Accion = "Editar", Permitido = true }
                });
                break;

            case "Vendedor":
                permisos.AddRange(new[]
                {
                    new Permiso { UsuarioId = usuarioId, Recurso = "Clientes", Accion = "Ver", Permitido = true },
                    new Permiso { UsuarioId = usuarioId, Recurso = "Clientes", Accion = "Editar", Permitido = true }
                });
                break;
        }

        _context.Permisos.AddRange(permisos);
        await _context.SaveChangesAsync();
    }

    private async Task RegistrarAuditoriaAcceso(int? usuarioId, string tipoAccion, string descripcion)
    {
        var auditoria = new AuditoriaAcceso
        {
            UsuarioId = usuarioId ?? 0,
            TipoAccion = tipoAccion,
            Descripcion = descripcion,
            IP = "TODO: Obtener IP real",
            UserAgent = "TODO: Obtener User Agent real"
        };

        _context.AuditoriaAccesos.Add(auditoria);
        await _context.SaveChangesAsync();
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPasswordHash(string password, string storedHash)
    {
        var hashOfInput = HashPassword(password);
        return storedHash == hashOfInput;
    }

    private string GeneratePasswordResetToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }

    private UsuarioDTO MapToUsuarioDTO(Usuario usuario)
    {
        return new UsuarioDTO
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Activo = usuario.Activo,
            FechaCreacion = usuario.FechaCreacion,
            UltimoAcceso = usuario.UltimoAcceso,
            Permisos = usuario.Permisos.Select(p => new PermisoDTO
            {
                Recurso = p.Recurso,
                Accion = p.Accion,
                Permitido = p.Permitido
            }).ToList()
        };
    }
} 