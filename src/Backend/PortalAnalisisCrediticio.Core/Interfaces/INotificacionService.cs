using System.Threading.Tasks;
using System.Collections.Generic;
using PortalAnalisisCrediticio.Shared.DTOs.Notificaciones;

namespace PortalAnalisisCrediticio.Core.Interfaces
{
    public interface INotificacionService
    {
        Task EnviarNotificacionEmailAsync(string destinatario, string asunto, string mensaje);
        Task EnviarNotificacionPushAsync(string usuarioId, string titulo, string mensaje);
        Task EnviarNotificacionMasivaAsync(List<string> destinatarios, string asunto, string mensaje);
        Task<List<NotificacionDTO>> ObtenerNotificacionesUsuarioAsync(string usuarioId);
        Task MarcarNotificacionComoLeidaAsync(string notificacionId);
        Task<int> ObtenerNotificacionesNoLeidasAsync(string usuarioId);
    }
} 