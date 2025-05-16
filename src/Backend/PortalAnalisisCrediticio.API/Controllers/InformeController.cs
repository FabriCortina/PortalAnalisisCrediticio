using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Informes;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de informes crediticios
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InformeController : ControllerBase
{
    private readonly IInformeService _informeService;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="informeService">Servicio de informes</param>
    public InformeController(IInformeService informeService)
    {
        _informeService = informeService;
    }

    /// <summary>
    /// Genera un informe crediticio para un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Informe crediticio generado</returns>
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

    /// <summary>
    /// Genera un informe crediticio en formato PDF
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Archivo PDF del informe</returns>
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

    /// <summary>
    /// Obtiene el historial de informes de un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Historial de informes</returns>
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