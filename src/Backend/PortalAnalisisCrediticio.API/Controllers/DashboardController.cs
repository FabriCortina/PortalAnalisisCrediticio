using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

namespace PortalAnalisisCrediticio.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IExportService _exportService;

        public DashboardController(IDashboardService dashboardService, IExportService exportService)
        {
            _dashboardService = dashboardService;
            _exportService = exportService;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDTO>> GetDashboard(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var dashboard = await _dashboardService.ObtenerDashboardAsync(fechaInicio, fechaFin);
            return Ok(dashboard);
        }

        [HttpGet("creditos-activos")]
        public async Task<ActionResult<List<CreditoActivoDTO>>> GetCreditosActivos(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var creditos = await _dashboardService.ObtenerCreditosActivosAsync(fechaInicio, fechaFin);
            return Ok(creditos);
        }

        [HttpGet("clientes-riesgo")]
        public async Task<ActionResult<List<ClienteRiesgoDTO>>> GetClientesRiesgo(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var clientes = await _dashboardService.ObtenerClientesRiesgoAsync(fechaInicio, fechaFin);
            return Ok(clientes);
        }

        [HttpGet("creditos-vencidos")]
        public async Task<ActionResult<List<CreditoVencidoDTO>>> GetCreditosVencidos(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var creditos = await _dashboardService.ObtenerCreditosVencidosAsync(fechaInicio, fechaFin);
            return Ok(creditos);
        }

        [HttpGet("metricas")]
        public async Task<ActionResult<MetricasDTO>> GetMetricas(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var metricas = await _dashboardService.ObtenerMetricasAsync(fechaInicio, fechaFin);
            return Ok(metricas);
        }

        [HttpGet("exportar/creditos-activos/excel")]
        public async Task<IActionResult> ExportarCreditosActivosExcel(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var bytes = await _exportService.ExportarCreditosActivosToExcelAsync(fechaInicio, fechaFin);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"creditos-activos-{fechaInicio:yyyyMMdd}-{fechaFin:yyyyMMdd}.xlsx");
        }

        [HttpGet("exportar/creditos-activos/csv")]
        public async Task<IActionResult> ExportarCreditosActivosCsv(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var bytes = await _exportService.ExportarCreditosActivosToCsvAsync(fechaInicio, fechaFin);
            return File(bytes, "text/csv", 
                $"creditos-activos-{fechaInicio:yyyyMMdd}-{fechaFin:yyyyMMdd}.csv");
        }

        [HttpGet("exportar/clientes-riesgo/excel")]
        public async Task<IActionResult> ExportarClientesRiesgoExcel(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var bytes = await _exportService.ExportarClientesRiesgoToExcelAsync(fechaInicio, fechaFin);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"clientes-riesgo-{fechaInicio:yyyyMMdd}-{fechaFin:yyyyMMdd}.xlsx");
        }

        [HttpGet("exportar/clientes-riesgo/csv")]
        public async Task<IActionResult> ExportarClientesRiesgoCsv(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var bytes = await _exportService.ExportarClientesRiesgoToCsvAsync(fechaInicio, fechaFin);
            return File(bytes, "text/csv", 
                $"clientes-riesgo-{fechaInicio:yyyyMMdd}-{fechaFin:yyyyMMdd}.csv");
        }

        [HttpGet("exportar/creditos-vencidos/excel")]
        public async Task<IActionResult> ExportarCreditosVencidosExcel(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var bytes = await _exportService.ExportarCreditosVencidosToExcelAsync(fechaInicio, fechaFin);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"creditos-vencidos-{fechaInicio:yyyyMMdd}-{fechaFin:yyyyMMdd}.xlsx");
        }

        [HttpGet("exportar/creditos-vencidos/csv")]
        public async Task<IActionResult> ExportarCreditosVencidosCsv(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var bytes = await _exportService.ExportarCreditosVencidosToCsvAsync(fechaInicio, fechaFin);
            return File(bytes, "text/csv", 
                $"creditos-vencidos-{fechaInicio:yyyyMMdd}-{fechaFin:yyyyMMdd}.csv");
        }

        [HttpGet("exportar/metricas/excel")]
        public async Task<IActionResult> ExportarMetricasExcel(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var bytes = await _exportService.ExportarMetricasToExcelAsync(fechaInicio, fechaFin);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"metricas-{fechaInicio:yyyyMMdd}-{fechaFin:yyyyMMdd}.xlsx");
        }

        [HttpGet("exportar/metricas/csv")]
        public async Task<IActionResult> ExportarMetricasCsv(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            var bytes = await _exportService.ExportarMetricasToCsvAsync(fechaInicio, fechaFin);
            return File(bytes, "text/csv", 
                $"metricas-{fechaInicio:yyyyMMdd}-{fechaFin:yyyyMMdd}.csv");
        }

        [HttpGet("admin")]
        public async Task<ActionResult<DashboardAdminDTO>> GetDashboardAdmin()
        {
            var dashboardAdmin = await _dashboardService.ObtenerDashboardAdminAsync();
            return Ok(dashboardAdmin);
        }
    }
} 