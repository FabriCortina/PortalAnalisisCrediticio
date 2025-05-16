using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de empresas
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmpresasController : ControllerBase
{
    private readonly IEmpresaService _empresaService;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="empresaService">Servicio de empresas</param>
    public EmpresasController(IEmpresaService empresaService)
    {
        _empresaService = empresaService;
    }

    /// <summary>
    /// Obtiene todas las empresas
    /// </summary>
    /// <returns>Lista de empresas</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmpresaDTO>>> GetAll()
    {
        var empresas = await _empresaService.GetAllAsync();
        return Ok(empresas);
    }

    /// <summary>
    /// Obtiene una empresa específica por su ID
    /// </summary>
    /// <param name="id">ID de la empresa</param>
    /// <returns>Datos de la empresa</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<EmpresaDTO>> GetById(int id)
    {
        var empresa = await _empresaService.GetByIdAsync(id);
        if (empresa == null)
            return NotFound();

        return Ok(empresa);
    }

    /// <summary>
    /// Crea una nueva empresa
    /// </summary>
    /// <param name="empresaDto">Datos de la empresa</param>
    /// <returns>Empresa creada</returns>
    [HttpPost]
    public async Task<ActionResult<EmpresaDTO>> Create(EmpresaDTO empresaDto)
    {
        var empresa = await _empresaService.CreateAsync(empresaDto);
        return CreatedAtAction(nameof(GetById), new { id = empresa.Id }, empresa);
    }

    /// <summary>
    /// Actualiza los datos de una empresa
    /// </summary>
    /// <param name="id">ID de la empresa</param>
    /// <param name="empresaDto">Datos actualizados de la empresa</param>
    /// <returns>Empresa actualizada</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<EmpresaDTO>> Update(int id, EmpresaDTO empresaDto)
    {
        var empresa = await _empresaService.UpdateAsync(id, empresaDto);
        if (empresa == null)
            return NotFound();

        return Ok(empresa);
    }

    /// <summary>
    /// Elimina una empresa
    /// </summary>
    /// <param name="id">ID de la empresa</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _empresaService.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{empresaId}/clientes/{clienteId}")]
    public async Task<ActionResult<ClienteEmpresaDTO>> AsociarCliente(int empresaId, int clienteId)
    {
        var asociacion = await _empresaService.AsociarClienteAsync(empresaId, clienteId);
        return Ok(asociacion);
    }

    [HttpDelete("{empresaId}/clientes/{clienteId}")]
    public async Task<ActionResult> DesasociarCliente(int empresaId, int clienteId)
    {
        var result = await _empresaService.DesasociarClienteAsync(empresaId, clienteId);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpGet("{id}/clientes")]
    public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes(int id)
    {
        var clientes = await _empresaService.GetClientesAsync(id);
        return Ok(clientes);
    }
} 