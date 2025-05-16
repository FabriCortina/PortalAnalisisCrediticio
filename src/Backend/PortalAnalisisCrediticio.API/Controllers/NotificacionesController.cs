using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Notificaciones;

namespace PortalAnalisisCrediticio.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;

        public NotificacionesController(INotificacionService notificacionService)
        {
            _notificacionService = notificacionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NotificacionDTO>>> GetNotificaciones()
        {
            var usuarioId = User.FindFirst("sub")?.Value;
            var notificaciones = await _notificacionService.ObtenerNotificacionesUsuarioAsync(usuarioId);
            return Ok(notificaciones);
        }

        [HttpGet("no-leidas")]
        public async Task<ActionResult<int>> GetNotificacionesNoLeidas()
        {
            var usuarioId = User.FindFirst("sub")?.Value;
            var cantidad = await _notificacionService.ObtenerNotificacionesNoLeidasAsync(usuarioId);
            return Ok(cantidad);
        }

        [HttpPost("marcar-leida/{id}")]
        public async Task<IActionResult> MarcarComoLeida(string id)
        {
            await _notificacionService.MarcarNotificacionComoLeidaAsync(id);
            return Ok();
        }

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