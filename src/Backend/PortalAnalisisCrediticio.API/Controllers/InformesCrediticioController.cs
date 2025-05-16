using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.InformeCrediticio;

namespace PortalAnalisisCrediticio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InformesCrediticioController : ControllerBase
{
    private readonly IInformeCrediticioService _informeService;
    private readonly ILogger<InformesCrediticioController> _logger;

    public InformesCrediticioController(
        IInformeCrediticioService informeService,
        ILogger<InformesCrediticioController> logger)
    {
        _informeService = informeService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InformeCrediticioDTO>> GetById(int id)
    {
        try
        {
            var informe = await _informeService.GetByIdAsync(id);
            if (informe == null)
                return NotFound();

            return Ok(informe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el informe crediticio {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<IEnumerable<InformeCrediticioDTO>>> GetByClienteId(int clienteId)
    {
        try
        {
            var informes = await _informeService.GetByClienteIdAsync(clienteId);
            return Ok(informes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los informes crediticios del cliente {ClienteId}", clienteId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<InformeCrediticioDTO>> Create(CreateInformeCrediticioDTO informeDto)
    {
        try
        {
            var informe = await _informeService.CreateAsync(informeDto);
            return CreatedAtAction(nameof(GetById), new { id = informe.Id }, informe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el informe crediticio");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<InformeCrediticioDTO>> Update(int id, UpdateInformeCrediticioDTO informeDto)
    {
        try
        {
            var informe = await _informeService.UpdateAsync(id, informeDto);
            if (informe == null)
                return NotFound();

            return Ok(informe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el informe crediticio {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _informeService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el informe crediticio {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> ExportarPDF(int id)
    {
        try
        {
            var pdfBytes = await _informeService.ExportarPDFAsync(id);
            return File(pdfBytes, "application/pdf", $"informe_crediticio_{id}.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al exportar el informe crediticio {Id} a PDF", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}/vista")]
    public async Task<ActionResult<string>> GenerarVistaWeb(int id)
    {
        try
        {
            var html = await _informeService.GenerarVistaWebAsync(id);
            return Content(html, "text/html");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar la vista web del informe crediticio {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 