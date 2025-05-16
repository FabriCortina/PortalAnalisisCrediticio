using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AnalisisController : ControllerBase
    {
        private readonly IAnalisisService _analisisService;
        private readonly ILogger<AnalisisController> _logger;

        public AnalisisController(IAnalisisService analisisService, ILogger<AnalisisController> logger)
        {
            _analisisService = analisisService;
            _logger = logger;
        }

        [HttpGet("metricas")]
        public async Task<ActionResult<MetricasAnalisisDto>> GetMetricas([FromQuery] int periodo)
        {
            try
            {
                var metricas = await _analisisService.GetMetricasAsync(periodo);
                return Ok(metricas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener métricas de análisis");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("distribucion-riesgos")]
        public async Task<ActionResult<DistribucionRiesgosDto>> GetDistribucionRiesgos([FromQuery] int periodo)
        {
            try
            {
                var distribucion = await _analisisService.GetDistribucionRiesgosAsync(periodo);
                return Ok(distribucion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener distribución de riesgos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("tendencias")]
        public async Task<ActionResult<TendenciasCreditoDto>> GetTendencias([FromQuery] int periodo)
        {
            try
            {
                var tendencias = await _analisisService.GetTendenciasCreditoAsync(periodo);
                return Ok(tendencias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tendencias de crédito");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("ultimos")]
        public async Task<ActionResult<IEnumerable<UltimoAnalisisDto>>> GetUltimosAnalisis()
        {
            try
            {
                var ultimosAnalisis = await _analisisService.GetUltimosAnalisisAsync();
                return Ok(ultimosAnalisis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener últimos análisis");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
} 