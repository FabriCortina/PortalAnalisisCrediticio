using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAll()
    {
        var clientes = await _clienteService.GetAllWithDetailsAsync();
        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDTO>> GetById(int id)
    {
        var cliente = await _clienteService.GetByIdWithDetailsAsync(id);
        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpGet("{id}/legajo")]
    public async Task<ActionResult<ClienteDTO>> GetLegajoDigital(int id)
    {
        var legajo = await _clienteService.GetLegajoDigitalAsync(id);
        if (legajo == null)
            return NotFound();

        return Ok(legajo);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDTO>> Create(ClienteDTO clienteDto)
    {
        var cliente = await _clienteService.CreateWithDetailsAsync(clienteDto);
        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClienteDTO>> Update(int id, ClienteDTO clienteDto)
    {
        var cliente = await _clienteService.UpdateWithDetailsAsync(id, clienteDto);
        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _clienteService.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost("importar")]
    public async Task<ActionResult<ClienteDTO>> ImportarDesdeExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No se ha proporcionado ningún archivo");

        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequest("El archivo debe ser un Excel (.xlsx)");

        using var stream = file.OpenReadStream();
        var cliente = await _clienteService.ImportarDesdeExcelAsync(stream);
        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
    }
} 