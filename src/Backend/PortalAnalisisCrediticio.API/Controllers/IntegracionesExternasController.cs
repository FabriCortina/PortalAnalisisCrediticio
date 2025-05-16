using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de integraciones con servicios externos
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class IntegracionesExternasController : ControllerBase
{
    private readonly IIntegracionesExternasService _integracionesExternasService;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="integracionesExternasService">Servicio de integraciones externas</param>
    public IntegracionesExternasController(IIntegracionesExternasService integracionesExternasService)
    {
        _integracionesExternasService = integracionesExternasService;
    }

    /// <summary>
    /// Obtiene el informe de Nosis para un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Informe de Nosis</returns>
    [HttpGet("nosis/{clienteId}")]
    public async Task<ActionResult<InformeNosisDTO>> GetInformeNosis(int clienteId)
    {
        var informe = await _integracionesExternasService.GetInformeNosisAsync(clienteId);
        return Ok(informe);
    }

    /// <summary>
    /// Obtiene el informe de Veraz para un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Informe de Veraz</returns>
    [HttpGet("veraz/{clienteId}")]
    public async Task<ActionResult<InformeVerazDTO>> GetInformeVeraz(int clienteId)
    {
        var informe = await _integracionesExternasService.GetInformeVerazAsync(clienteId);
        return Ok(informe);
    }

    /// <summary>
    /// Obtiene el informe de Infoexperto para un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Informe de Infoexperto</returns>
    [HttpGet("infoexperto/{clienteId}")]
    public async Task<ActionResult<InformeInfoexpertoDTO>> GetInformeInfoexperto(int clienteId)
    {
        var informe = await _integracionesExternasService.GetInformeInfoexpertoAsync(clienteId);
        return Ok(informe);
    }

    /// <summary>
    /// Obtiene el informe del BCRA para un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Informe del BCRA</returns>
    [HttpGet("bcra/{clienteId}")]
    public async Task<ActionResult<InformeBCRADTO>> GetInformeBCRA(int clienteId)
    {
        var informe = await _integracionesExternasService.GetInformeBCRAAsync(clienteId);
        return Ok(informe);
    }

    /// <summary>
    /// Obtiene el informe de AFIP para un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Informe de AFIP</returns>
    [HttpGet("afip/{clienteId}")]
    public async Task<ActionResult<InformeAFIPDTO>> GetInformeAFIP(int clienteId)
    {
        var informe = await _integracionesExternasService.GetInformeAFIPAsync(clienteId);
        return Ok(informe);
    }

    [HttpGet("publico/{clienteId}")]
    public async Task<ActionResult<InformePublicoDTO>> GetInformePublico(int clienteId)
    {
        var informe = await _integracionesExternasService.GetInformePublicoAsync(clienteId);
        return Ok(informe);
    }
} 