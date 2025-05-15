using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolicitudesProductoController : ControllerBase
{
    private readonly ISolicitudProductoService _solicitudService;

    public SolicitudesProductoController(ISolicitudProductoService solicitudService)
    {
        _solicitudService = solicitudService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SolicitudProductoDTO>>> GetAll()
    {
        var solicitudes = await _solicitudService.GetAllAsync();
        return Ok(solicitudes);
    }

    [HttpGet("pendientes")]
    public async Task<ActionResult<IEnumerable<SolicitudProductoDTO>>> GetPendientes()
    {
        var solicitudes = await _solicitudService.GetPendientesAsync();
        return Ok(solicitudes);
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<IEnumerable<SolicitudProductoDTO>>> GetByClienteId(int clienteId)
    {
        var solicitudes = await _solicitudService.GetByClienteIdAsync(clienteId);
        return Ok(solicitudes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SolicitudProductoDTO>> GetById(int id)
    {
        var solicitud = await _solicitudService.GetByIdAsync(id);
        if (solicitud == null)
            return NotFound();

        return Ok(solicitud);
    }

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