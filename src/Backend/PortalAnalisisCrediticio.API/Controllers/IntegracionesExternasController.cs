using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

namespace PortalAnalisisCrediticio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntegracionesExternasController : ControllerBase
{
    private readonly IIntegracionesExternasService _integracionesExternasService;

    public IntegracionesExternasController(IIntegracionesExternasService integracionesExternasService)
    {
        _integracionesExternasService = integracionesExternasService;
    }

    [HttpGet("nosis/{clienteId}")]
    public async Task<ActionResult<InformeNosisDTO>> GetInformeNosis(int clienteId)
    {
        var informe = await _integracionesExternasService.GetInformeNosisAsync(clienteId);
        return Ok(informe);
    }

    [HttpGet("veraz/{clienteId}")]
    public async Task<ActionResult<InformeVerazDTO>> GetInformeVeraz(int clienteId)
    {
        var informe = await _integracionesExternasService.GetInformeVerazAsync(clienteId);
        return Ok(informe);
    }

    [HttpGet("infoexperto/{clienteId}")]
    public async Task<ActionResult<InformeInfoexpertoDTO>> GetInformeInfoexperto(int clienteId)
    {
        var informe = await _integracionesExternasService.GetInformeInfoexpertoAsync(clienteId);
        return Ok(informe);
    }

    [HttpGet("bcra/{clienteId}")]
    public async Task<ActionResult<InformeBCRADTO>> GetInformeBCRA(int clienteId)
    {
        var informe = await _integracionesExternasService.GetInformeBCRAAsync(clienteId);
        return Ok(informe);
    }

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