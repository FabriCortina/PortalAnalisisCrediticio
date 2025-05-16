using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de solicitudes de productos financieros
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SolicitudesProductoController : ControllerBase
{
    private readonly ISolicitudProductoService _solicitudService;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="solicitudService">Servicio de solicitudes de producto</param>
    public SolicitudesProductoController(ISolicitudProductoService solicitudService)
    {
        _solicitudService = solicitudService;
    }

    /// <summary>
    /// Obtiene todas las solicitudes de productos
    /// </summary>
    /// <returns>Lista de solicitudes</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SolicitudProductoDTO>>> GetAll()
    {
        var solicitudes = await _solicitudService.GetAllAsync();
        return Ok(solicitudes);
    }

    /// <summary>
    /// Obtiene las solicitudes pendientes de aprobación
    /// </summary>
    /// <returns>Lista de solicitudes pendientes</returns>
    [HttpGet("pendientes")]
    public async Task<ActionResult<IEnumerable<SolicitudProductoDTO>>> GetPendientes()
    {
        var solicitudes = await _solicitudService.GetPendientesAsync();
        return Ok(solicitudes);
    }

    /// <summary>
    /// Obtiene las solicitudes de un cliente específico
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Lista de solicitudes del cliente</returns>
    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<IEnumerable<SolicitudProductoDTO>>> GetByClienteId(int clienteId)
    {
        var solicitudes = await _solicitudService.GetByClienteIdAsync(clienteId);
        return Ok(solicitudes);
    }

    /// <summary>
    /// Obtiene una solicitud específica por su ID
    /// </summary>
    /// <param name="id">ID de la solicitud</param>
    /// <returns>Datos de la solicitud</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SolicitudProductoDTO>> GetById(int id)
    {
        var solicitud = await _solicitudService.GetByIdAsync(id);
        if (solicitud == null)
            return NotFound();

        return Ok(solicitud);
    }

    /// <summary>
    /// Crea una nueva solicitud de producto
    /// </summary>
    /// <param name="solicitudDto">Datos de la solicitud</param>
    /// <returns>Solicitud creada</returns>
    [HttpPost]
    public async Task<ActionResult<SolicitudProductoDTO>> Create(CreateSolicitudProductoDTO solicitudDto)
    {
        try
        {
            var solicitud = await _solicitudService.CreateAsync(solicitudDto);
            return CreatedAtAction(nameof(GetById), new { id = solicitud.Id }, solicitud);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SolicitudProductoDTO>> Update(int id, UpdateSolicitudProductoDTO solicitudDto)
    {
        try
        {
            var solicitud = await _solicitudService.UpdateAsync(id, solicitudDto);
            if (solicitud == null)
                return NotFound();

            return Ok(solicitud);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _solicitudService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/estado")]
    public async Task<ActionResult<SolicitudProductoDTO>> CambiarEstado(int id, [FromBody] string nuevoEstado)
    {
        var solicitud = await _solicitudService.CambiarEstadoAsync(id, nuevoEstado);
        if (solicitud == null)
            return NotFound();

        return Ok(solicitud);
    }
} 