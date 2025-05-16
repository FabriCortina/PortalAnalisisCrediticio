using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.InformacionFinanciera;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de información financiera de clientes
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InformacionFinancieraController : ControllerBase
{
    private readonly IInformacionFinancieraService _informacionFinancieraService;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="informacionFinancieraService">Servicio de información financiera</param>
    public InformacionFinancieraController(IInformacionFinancieraService informacionFinancieraService)
    {
        _informacionFinancieraService = informacionFinancieraService;
    }

    /// <summary>
    /// Obtiene toda la información financiera
    /// </summary>
    /// <returns>Lista de información financiera</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InformacionFinancieraDTO>>> GetAll()
    {
        var informacionFinanciera = await _informacionFinancieraService.GetAllAsync();
        return Ok(informacionFinanciera);
    }

    /// <summary>
    /// Obtiene la información financiera por su ID
    /// </summary>
    /// <param name="id">ID de la información financiera</param>
    /// <returns>Datos de la información financiera</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<InformacionFinancieraDTO>> GetById(int id)
    {
        var informacionFinanciera = await _informacionFinancieraService.GetByIdAsync(id);
        if (informacionFinanciera == null)
            return NotFound();

        return Ok(informacionFinanciera);
    }

    /// <summary>
    /// Obtiene la información financiera de un cliente específico
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Información financiera del cliente</returns>
    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<InformacionFinancieraDTO>> GetByClienteId(int clienteId)
    {
        var informacion = await _informacionFinancieraService.GetByClienteIdAsync(clienteId);
        if (informacion == null)
            return NotFound();

        return Ok(informacion);
    }

    /// <summary>
    /// Crea nueva información financiera para un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <param name="informacionDto">Datos de la información financiera</param>
    /// <returns>Información financiera creada</returns>
    [HttpPost("cliente/{clienteId}")]
    public async Task<ActionResult<InformacionFinancieraDTO>> Create(int clienteId, InformacionFinancieraDTO informacionDto)
    {
        var informacion = await _informacionFinancieraService.CreateAsync(clienteId, informacionDto);
        return CreatedAtAction(nameof(GetByClienteId), new { clienteId }, informacion);
    }

    /// <summary>
    /// Actualiza la información financiera de un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <param name="informacionDto">Datos actualizados de la información financiera</param>
    /// <returns>Información financiera actualizada</returns>
    [HttpPut("cliente/{clienteId}")]
    public async Task<ActionResult<InformacionFinancieraDTO>> Update(int clienteId, InformacionFinancieraDTO informacionDto)
    {
        var informacion = await _informacionFinancieraService.UpdateAsync(clienteId, informacionDto);
        if (informacion == null)
            return NotFound();

        return Ok(informacion);
    }

    /// <summary>
    /// Elimina la información financiera de un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("cliente/{clienteId}")]
    public async Task<ActionResult> Delete(int clienteId)
    {
        await _informacionFinancieraService.DeleteAsync(clienteId);
        return NoContent();
    }

    /// <summary>
    /// Agrega un estado financiero a un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <param name="estadoDto">Datos del estado financiero</param>
    /// <returns>Estado financiero agregado</returns>
    [HttpPost("cliente/{clienteId}/estados-financieros")]
    public async Task<ActionResult<EstadoFinancieroDTO>> AddEstadoFinanciero(int clienteId, EstadoFinancieroDTO estadoDto)
    {
        var estado = await _informacionFinancieraService.AddEstadoFinancieroAsync(clienteId, estadoDto);
        return Ok(estado);
    }

    /// <summary>
    /// Agrega un flujo de caja proyectado a un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <param name="flujoDto">Datos del flujo de caja</param>
    /// <returns>Flujo de caja agregado</returns>
    [HttpPost("cliente/{clienteId}/flujos-caja")]
    public async Task<ActionResult<FlujoCajaProyectadoDTO>> AddFlujoCaja(int clienteId, FlujoCajaProyectadoDTO flujoDto)
    {
        var flujo = await _informacionFinancieraService.AddFlujoCajaAsync(clienteId, flujoDto);
        return Ok(flujo);
    }

    /// <summary>
    /// Agrega una deuda a un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <param name="deudaDto">Datos de la deuda</param>
    /// <returns>Deuda agregada</returns>
    [HttpPost("cliente/{clienteId}/deudas")]
    public async Task<ActionResult<DeudaDTO>> AddDeuda(int clienteId, DeudaDTO deudaDto)
    {
        var deuda = await _informacionFinancieraService.AddDeudaAsync(clienteId, deudaDto);
        return Ok(deuda);
    }

    /// <summary>
    /// Importa información financiera desde un archivo Excel
    /// </summary>
    /// <param name="file">Archivo Excel con la información financiera</param>
    /// <returns>Información financiera importada</returns>
    [HttpPost("importar-excel")]
    public async Task<ActionResult<InformacionFinancieraDTO>> ImportarExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No se ha proporcionado ningún archivo");

        var informacion = await _informacionFinancieraService.ImportarExcelAsync(file);
        return Ok(informacion);
    }
} 