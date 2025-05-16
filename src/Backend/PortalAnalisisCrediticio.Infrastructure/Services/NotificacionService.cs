using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.Notificaciones;

namespace PortalAnalisisCrediticio.Infrastructure.Services
{
    public class NotificacionService : INotificacionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<NotificacionService> _logger;
        private const int MaxRetries = 3;
        private const int RetryDelayMs = 1000;

        public NotificacionService(
            ApplicationDbContext context,
            IEmailService emailService,
            ILogger<NotificacionService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task EnviarNotificacionEmailAsync(string destinatario, string asunto, string mensaje)
        {
            try
            {
                if (string.IsNullOrEmpty(destinatario))
                    throw new ArgumentException("El destinatario no puede estar vacío", nameof(destinatario));

                if (string.IsNullOrEmpty(asunto))
                    throw new ArgumentException("El asunto no puede estar vacío", nameof(asunto));

                if (string.IsNullOrEmpty(mensaje))
                    throw new ArgumentException("El mensaje no puede estar vacío", nameof(mensaje));

                var retryCount = 0;
                while (retryCount < MaxRetries)
                {
                    try
                    {
                        await _emailService.EnviarEmailNotificacionAsync(destinatario, asunto, mensaje);
                        _logger.LogInformation($"Notificación email enviada exitosamente a {destinatario}");
                        return;
                    }
                    catch (Exception ex)
                    {
                        retryCount++;
                        if (retryCount == MaxRetries)
                            throw;

                        _logger.LogWarning($"Error al enviar notificación email a {destinatario}. Reintento {retryCount} de {MaxRetries}. Error: {ex.Message}");
                        await Task.Delay(RetryDelayMs * retryCount);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al enviar notificación email a {destinatario}: {ex.Message}");
                throw new Exception($"Error al enviar notificación email: {ex.Message}", ex);
            }
        }

        public async Task EnviarNotificacionPushAsync(string usuarioId, string titulo, string mensaje)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioId))
                    throw new ArgumentException("El ID de usuario no puede estar vacío", nameof(usuarioId));

                if (string.IsNullOrEmpty(titulo))
                    throw new ArgumentException("El título no puede estar vacío", nameof(titulo));

                if (string.IsNullOrEmpty(mensaje))
                    throw new ArgumentException("El mensaje no puede estar vacío", nameof(mensaje));

                var notificacion = new Notificacion
                {
                    Id = Guid.NewGuid().ToString(),
                    UsuarioId = usuarioId,
                    Titulo = titulo,
                    Mensaje = mensaje,
                    Tipo = "Push",
                    FechaCreacion = DateTime.UtcNow,
                    Leida = false
                };

                _context.Notificaciones.Add(notificacion);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Notificación push enviada exitosamente al usuario {usuarioId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al enviar notificación push al usuario {usuarioId}: {ex.Message}");
                throw new Exception($"Error al enviar notificación push: {ex.Message}", ex);
            }
        }

        public async Task EnviarNotificacionMasivaAsync(List<string> destinatarios, string asunto, string mensaje)
        {
            try
            {
                if (destinatarios == null || !destinatarios.Any())
                    throw new ArgumentException("La lista de destinatarios no puede estar vacía", nameof(destinatarios));

                if (string.IsNullOrEmpty(asunto))
                    throw new ArgumentException("El asunto no puede estar vacío", nameof(asunto));

                if (string.IsNullOrEmpty(mensaje))
                    throw new ArgumentException("El mensaje no puede estar vacío", nameof(mensaje));

                var tasks = new List<Task>();
                foreach (var destinatario in destinatarios)
                {
                    tasks.Add(EnviarNotificacionEmailAsync(destinatario, asunto, mensaje));
                }

                await Task.WhenAll(tasks);
                _logger.LogInformation($"Notificaciones masivas enviadas exitosamente a {destinatarios.Count} destinatarios");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al enviar notificaciones masivas: {ex.Message}");
                throw new Exception($"Error al enviar notificaciones masivas: {ex.Message}", ex);
            }
        }

        public async Task<List<NotificacionDTO>> ObtenerNotificacionesUsuarioAsync(string usuarioId)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioId))
                    throw new ArgumentException("El ID de usuario no puede estar vacío", nameof(usuarioId));

                var notificaciones = await _context.Notificaciones
                    .Where(n => n.UsuarioId == usuarioId)
                    .OrderByDescending(n => n.FechaCreacion)
                    .Select(n => new NotificacionDTO
                    {
                        Id = n.Id,
                        UsuarioId = n.UsuarioId,
                        Titulo = n.Titulo,
                        Mensaje = n.Mensaje,
                        Tipo = n.Tipo,
                        FechaCreacion = n.FechaCreacion,
                        FechaLectura = n.FechaLectura,
                        Leida = n.Leida,
                        Enlace = n.Enlace
                    })
                    .ToListAsync();

                _logger.LogInformation($"Se obtuvieron {notificaciones.Count} notificaciones para el usuario {usuarioId}");
                return notificaciones;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener notificaciones del usuario {usuarioId}: {ex.Message}");
                throw new Exception($"Error al obtener notificaciones: {ex.Message}", ex);
            }
        }

        public async Task MarcarNotificacionComoLeidaAsync(string notificacionId)
        {
            try
            {
                if (string.IsNullOrEmpty(notificacionId))
                    throw new ArgumentException("El ID de notificación no puede estar vacío", nameof(notificacionId));

                var notificacion = await _context.Notificaciones.FindAsync(notificacionId);
                if (notificacion == null)
                    throw new Exception($"No se encontró la notificación con ID {notificacionId}");

                notificacion.Leida = true;
                notificacion.FechaLectura = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Notificación {notificacionId} marcada como leída");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al marcar notificación {notificacionId} como leída: {ex.Message}");
                throw new Exception($"Error al marcar notificación como leída: {ex.Message}", ex);
            }
        }

        public async Task<int> ObtenerNotificacionesNoLeidasAsync(string usuarioId)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioId))
                    throw new ArgumentException("El ID de usuario no puede estar vacío", nameof(usuarioId));

                var count = await _context.Notificaciones
                    .CountAsync(n => n.UsuarioId == usuarioId && !n.Leida);

                _logger.LogInformation($"El usuario {usuarioId} tiene {count} notificaciones no leídas");
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener notificaciones no leídas del usuario {usuarioId}: {ex.Message}");
                throw new Exception($"Error al obtener notificaciones no leídas: {ex.Message}", ex);
            }
        }
    }
} 