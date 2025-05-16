using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Auditoria;

namespace PortalAnalisisCrediticio.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de auditoría del sistema
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaService _auditoriaService;

        /// <summary>
        /// Constructor del controlador
        /// </summary>
        /// <param name="auditoriaService">Servicio de auditoría</param>
        public AuditoriaController(IAuditoriaService auditoriaService)
        {
            _auditoriaService = auditoriaService;
        }

        /// <summary>
        /// Obtiene las actividades de auditoría en un rango de fechas
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <param name="usuarioId">ID del usuario (opcional)</param>
        /// <returns>Lista de actividades</returns>
        [HttpGet]
        public async Task<ActionResult<List<ActividadDTO>>> GetActividades(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin,
            [FromQuery] string usuarioId = null)
        {
            var actividades = await _auditoriaService.ObtenerActividadesAsync(fechaInicio, fechaFin, usuarioId);
            return Ok(actividades);
        }

        /// <summary>
        /// Obtiene las actividades de auditoría por tipo en un rango de fechas
        /// </summary>
        /// <param name="tipoActividad">Tipo de actividad</param>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>Lista de actividades</returns>
        [HttpGet("por-tipo")]
        public async Task<ActionResult<List<ActividadDTO>>> GetActividadesPorTipo(
            [FromQuery] string tipoActividad,
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var actividades = await _auditoriaService.ObtenerActividadesPorTipoAsync(tipoActividad, fechaInicio, fechaFin);
            return Ok(actividades);
        }

        /// <summary>
        /// Obtiene estadísticas de auditoría en un rango de fechas
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>Estadísticas de auditoría</returns>
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