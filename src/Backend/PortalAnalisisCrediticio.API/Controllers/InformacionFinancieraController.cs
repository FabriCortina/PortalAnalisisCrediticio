using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.InformacionFinanciera;

namespace PortalAnalisisCrediticio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InformacionFinancieraController : ControllerBase
{
    private readonly IInformacionFinancieraService _informacionFinancieraService;

    public InformacionFinancieraController(IInformacionFinancieraService informacionFinancieraService)
    {
        _informacionFinancieraService = informacionFinancieraService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InformacionFinancieraDTO>>> GetAll()
    {
        var informacionFinanciera = await _informacionFinancieraService.GetAllAsync();
        return Ok(informacionFinanciera);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InformacionFinancieraDTO>> GetById(int id)
    {
        var informacionFinanciera = await _informacionFinancieraService.GetByIdAsync(id);
        if (informacionFinanciera == null)
            return NotFound();

        return Ok(informacionFinanciera);
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<InformacionFinancieraDTO>> GetByClienteId(int clienteId)
    {
        var informacion = await _informacionFinancieraService.GetByClienteIdAsync(clienteId);
        if (informacion == null)
            return NotFound();

        return Ok(informacion);
    }

    [HttpPost("cliente/{clienteId}")]
    public async Task<ActionResult<InformacionFinancieraDTO>> Create(int clienteId, InformacionFinancieraDTO informacionDto)
    {
        var informacion = await _informacionFinancieraService.CreateAsync(clienteId, informacionDto);
        return CreatedAtAction(nameof(GetByClienteId), new { clienteId }, informacion);
    }

    [HttpPut("cliente/{clienteId}")]
    public async Task<ActionResult<InformacionFinancieraDTO>> Update(int clienteId, InformacionFinancieraDTO informacionDto)
    {
        var informacion = await _informacionFinancieraService.UpdateAsync(clienteId, informacionDto);
        if (informacion == null)
            return NotFound();

        return Ok(informacion);
    }

    [HttpDelete("cliente/{clienteId}")]
    public async Task<ActionResult> Delete(int clienteId)
    {
        await _informacionFinancieraService.DeleteAsync(clienteId);
        return NoContent();
    }

    [HttpPost("cliente/{clienteId}/estados-financieros")]
    public async Task<ActionResult<EstadoFinancieroDTO>> AddEstadoFinanciero(int clienteId, EstadoFinancieroDTO estadoDto)
    {
        var estado = await _informacionFinancieraService.AddEstadoFinancieroAsync(clienteId, estadoDto);
        return Ok(estado);
    }

    [HttpPost("cliente/{clienteId}/flujos-caja")]
    public async Task<ActionResult<FlujoCajaProyectadoDTO>> AddFlujoCaja(int clienteId, FlujoCajaProyectadoDTO flujoDto)
    {
        var flujo = await _informacionFinancieraService.AddFlujoCajaAsync(clienteId, flujoDto);
        return Ok(flujo);
    }

    [HttpPost("cliente/{clienteId}/deudas")]
    public async Task<ActionResult<DeudaDTO>> AddDeuda(int clienteId, DeudaDTO deudaDto)
    {
        var deuda = await _informacionFinancieraService.AddDeudaAsync(clienteId, deudaDto);
        return Ok(deuda);
    }

    [HttpPost("importar-excel")]
    public async Task<ActionResult<InformacionFinancieraDTO>> ImportarExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No se ha proporcionado ningún archivo");

        using var stream = file.OpenReadStream();
        var informacionFinanciera = await _informacionFinancieraService.ImportarDesdeExcelAsync(stream);
        return CreatedAtAction(nameof(GetById), new { id = informacionFinanciera.Id }, informacionFinanciera);
    }
} 