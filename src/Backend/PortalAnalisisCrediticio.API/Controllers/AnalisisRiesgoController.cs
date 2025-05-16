using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.AnalisisRiesgo;

namespace PortalAnalisisCrediticio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalisisRiesgoController : ControllerBase
{
    private readonly IAnalisisRiesgoService _analisisRiesgoService;
    private readonly ILogger<AnalisisRiesgoController> _logger;

    public AnalisisRiesgoController(
        IAnalisisRiesgoService analisisRiesgoService,
        ILogger<AnalisisRiesgoController> logger)
    {
        _analisisRiesgoService = analisisRiesgoService;
        _logger = logger;
    }

    [HttpPost("analizar")]
    public async Task<ActionResult<AnalisisRiesgoResponseDTO>> AnalizarRiesgo(AnalisisRiesgoRequestDTO request)
    {
        try
        {
            var resultado = await _analisisRiesgoService.AnalizarRiesgoAsync(request);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al realizar el análisis de riesgo");
            return StatusCode(500, "Error interno del servidor al realizar el análisis de riesgo");
        }
    }

    [HttpPost("realizar/{clienteId}")]
    public async Task<ActionResult<InformeRiesgoDTO>> RealizarAnalisisRiesgo(int clienteId)
    {
        try
        {
            var informe = await _analisisRiesgoService.RealizarAnalisisRiesgoAsync(clienteId);
            return Ok(informe);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet("{clienteId}")]
    public async Task<ActionResult<InformeRiesgoDTO>> GetInformeRiesgo(int clienteId)
    {
        try
        {
            var informe = await _analisisRiesgoService.GetInformeRiesgoAsync(clienteId);
            return Ok(informe);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet("exportar-pdf/{clienteId}")]
    public async Task<IActionResult> ExportarInformePDF(int clienteId)
    {
        try
        {
            var pdfBytes = await _analisisRiesgoService.ExportarInformePDFAsync(clienteId);
            return File(pdfBytes, "application/pdf", $"informe-riesgo-{clienteId}.pdf");
        }
        catch (NotImplementedException)
        {
            return BadRequest(new { mensaje = "La exportación a PDF aún no está implementada" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
} 