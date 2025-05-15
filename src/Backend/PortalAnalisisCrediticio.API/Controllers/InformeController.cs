using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Informes;

namespace PortalAnalisisCrediticio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InformeController : ControllerBase
{
    private readonly IInformeService _informeService;

    public InformeController(IInformeService informeService)
    {
        _informeService = informeService;
    }

    [HttpGet("{clienteId}")]
    public async Task<ActionResult<InformeDTO>> GetInforme(int clienteId)
    {
        try
        {
            var informe = await _informeService.GenerarInformeAsync(clienteId);
            return Ok(informe);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet("{clienteId}/pdf")]
    public async Task<IActionResult> GetInformePDF(int clienteId)
    {
        try
        {
            var pdfBytes = await _informeService.ExportarInformePDFAsync(clienteId);
            return File(pdfBytes, "application/pdf", $"informe_riesgo_{clienteId}.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet("{clienteId}/historial")]
    public async Task<ActionResult<IEnumerable<InformeHistoricoDTO>>> GetHistorialInformes(int clienteId)
    {
        try
        {
            var historial = await _informeService.GetHistorialInformesAsync(clienteId);
            return Ok(historial);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
} 