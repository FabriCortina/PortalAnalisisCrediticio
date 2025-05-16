using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Notificaciones;

namespace PortalAnalisisCrediticio.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de notificaciones del sistema
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;

        /// <summary>
        /// Constructor del controlador
        /// </summary>
        /// <param name="notificacionService">Servicio de notificaciones</param>
        public NotificacionesController(INotificacionService notificacionService)
        {
            _notificacionService = notificacionService;
        }

        /// <summary>
        /// Obtiene las notificaciones del usuario actual
        /// </summary>
        /// <returns>Lista de notificaciones</returns>
        [HttpGet]
        public async Task<ActionResult<List<NotificacionDTO>>> GetNotificaciones()
        {
            var usuarioId = User.FindFirst("sub")?.Value;
            var notificaciones = await _notificacionService.ObtenerNotificacionesUsuarioAsync(usuarioId);
            return Ok(notificaciones);
        }

        /// <summary>
        /// Obtiene la cantidad de notificaciones no leídas del usuario actual
        /// </summary>
        /// <returns>Cantidad de notificaciones no leídas</returns>
        [HttpGet("no-leidas")]
        public async Task<ActionResult<int>> GetNotificacionesNoLeidas()
        {
            var usuarioId = User.FindFirst("sub")?.Value;
            var cantidad = await _notificacionService.ObtenerNotificacionesNoLeidasAsync(usuarioId);
            return Ok(cantidad);
        }

        /// <summary>
        /// Marca una notificación como leída
        /// </summary>
        /// <param name="id">ID de la notificación</param>
        /// <returns>Sin contenido</returns>
        [HttpPost("marcar-leida/{id}")]
        public async Task<IActionResult> MarcarComoLeida(string id)
        {
            await _notificacionService.MarcarNotificacionComoLeidaAsync(id);
            return Ok();
        }

        /// <summary>
        /// Envía una notificación a usuarios específicos
        /// </summary>
        /// <param name="request">Datos de la notificación</param>
        /// <returns>Sin contenido</returns>
        [Authorize(Roles = "Administrador")]
        [HttpPost("enviar")]
        public async Task<IActionResult> EnviarNotificacion([FromBody] NotificacionRequestDTO request)
        {
            foreach (var usuarioId in request.UsuariosDestino)
            {
                await _notificacionService.EnviarNotificacionPushAsync(usuarioId, request.Titulo, request.Mensaje);
            }
            return Ok();
        }
    }
} 