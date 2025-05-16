using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de clientes
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="clienteService">Servicio de clientes</param>
    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    /// <summary>
    /// Obtiene todos los clientes con sus detalles
    /// </summary>
    /// <returns>Lista de clientes</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAll()
    {
        var clientes = await _clienteService.GetAllWithDetailsAsync();
        return Ok(clientes);
    }

    /// <summary>
    /// Obtiene un cliente específico con sus detalles
    /// </summary>
    /// <param name="id">ID del cliente</param>
    /// <returns>Datos del cliente</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDTO>> GetById(int id)
    {
        var cliente = await _clienteService.GetByIdWithDetailsAsync(id);
        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    /// <summary>
    /// Obtiene el legajo digital de un cliente
    /// </summary>
    /// <param name="id">ID del cliente</param>
    /// <returns>Legajo digital del cliente</returns>
    [HttpGet("{id}/legajo")]
    public async Task<ActionResult<ClienteDTO>> GetLegajoDigital(int id)
    {
        var legajo = await _clienteService.GetLegajoDigitalAsync(id);
        if (legajo == null)
            return NotFound();

        return Ok(legajo);
    }

    /// <summary>
    /// Crea un nuevo cliente con sus detalles
    /// </summary>
    /// <param name="clienteDto">Datos del cliente</param>
    /// <returns>Cliente creado</returns>
    [HttpPost]
    public async Task<ActionResult<ClienteDTO>> Create(ClienteDTO clienteDto)
    {
        var cliente = await _clienteService.CreateWithDetailsAsync(clienteDto);
        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
    }

    /// <summary>
    /// Actualiza los datos de un cliente
    /// </summary>
    /// <param name="id">ID del cliente</param>
    /// <param name="clienteDto">Datos actualizados del cliente</param>
    /// <returns>Cliente actualizado</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ClienteDTO>> Update(int id, ClienteDTO clienteDto)
    {
        var cliente = await _clienteService.UpdateWithDetailsAsync(id, clienteDto);
        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    /// <summary>
    /// Elimina un cliente
    /// </summary>
    /// <param name="id">ID del cliente</param>
    /// <returns>Sin contenido</returns>
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