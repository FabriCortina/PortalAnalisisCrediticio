using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de análisis de riesgo crediticio
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalisisRiesgoController : ControllerBase
{
    private readonly IAnalisisRiesgoService _analisisService;
    private readonly ILogService _logService;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="analisisService">Servicio de análisis de riesgo</param>
    /// <param name="logService">Servicio de logs</param>
    public AnalisisRiesgoController(IAnalisisRiesgoService analisisService, ILogService logService)
    {
        _analisisService = analisisService;
        _logService = logService;
    }

    /// <summary>
    /// Realiza un análisis de riesgo para un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Informe de riesgo</returns>
    [HttpPost("cliente/{clienteId}")]
    public async Task<ActionResult<InformeRiesgoDTO>> RealizarAnalisis(int clienteId)
    {
        var informe = await _analisisService.RealizarAnalisisRiesgoAsync(clienteId);
        await _logService.RegistrarLogAsync(new LogDTO
        {
            Accion = "Análisis de Riesgo",
            Detalles = $"Análisis realizado para cliente: {clienteId}",
            Usuario = User.Identity.Name
        });

        return Ok(informe);
    }

    /// <summary>
    /// Obtiene el último informe de riesgo de un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Último informe de riesgo</returns>
    [HttpGet("cliente/{clienteId}/ultimo")]
    public async Task<ActionResult<InformeRiesgoDTO>> GetUltimoInforme(int clienteId)
    {
        var informe = await _analisisService.ObtenerUltimoInformeAsync(clienteId);
        if (informe == null)
            return NotFound();

        return Ok(informe);
    }

    /// <summary>
    /// Obtiene el historial de informes de riesgo de un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Historial de informes</returns>
    [HttpGet("cliente/{clienteId}/historial")]
    public async Task<ActionResult<IEnumerable<InformeRiesgoDTO>>> GetHistorialInformes(int clienteId)
    {
        var historial = await _analisisService.ObtenerHistorialInformesAsync(clienteId);
        return Ok(historial);
    }

    /// <summary>
    /// Genera un PDF del informe de riesgo
    /// </summary>
    /// <param name="informeId">ID del informe</param>
    /// <returns>Archivo PDF del informe</returns>
    [HttpGet("informe/{informeId}/pdf")]
    public async Task<ActionResult> GenerarPDF(int informeId)
    {
        var pdfBytes = await _analisisService.GenerarInformePDFAsync(informeId);
        if (pdfBytes == null)
            return NotFound();

        await _logService.RegistrarLogAsync(new LogDTO
        {
            Accion = "Generar PDF",
            Detalles = $"PDF generado para informe: {informeId}",
            Usuario = User.Identity.Name
        });

        return File(pdfBytes, "application/pdf", $"informe-riesgo-{informeId}.pdf");
    }
} 