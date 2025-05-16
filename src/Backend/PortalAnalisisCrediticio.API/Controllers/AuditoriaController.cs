using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Auditoria;

namespace PortalAnalisisCrediticio.API.Controllers
{
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaService _auditoriaService;

        public AuditoriaController(IAuditoriaService auditoriaService)
        {
            _auditoriaService = auditoriaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActividadDTO>>> GetActividades(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin,
            [FromQuery] string usuarioId = null)
        {
            var actividades = await _auditoriaService.ObtenerActividadesAsync(fechaInicio, fechaFin, usuarioId);
            return Ok(actividades);
        }

        [HttpGet("por-tipo")]
        public async Task<ActionResult<List<ActividadDTO>>> GetActividadesPorTipo(
            [FromQuery] string tipoActividad,
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var actividades = await _auditoriaService.ObtenerActividadesPorTipoAsync(tipoActividad, fechaInicio, fechaFin);
            return Ok(actividades);
        }

        [HttpGet("estadisticas")]
        public async Task<ActionResult<EstadisticasAuditoriaDTO>> GetEstadisticas(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var estadisticas = await _auditoriaService.ObtenerEstadisticasAsync(fechaInicio, fechaFin);
            return Ok(estadisticas);
        }
    }
} 