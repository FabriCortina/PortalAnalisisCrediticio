using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Producto;

namespace PortalAnalisisCrediticio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;
    private readonly ILogger<ProductosController> _logger;

    public ProductosController(
        IProductoService productoService,
        ILogger<ProductosController> logger)
    {
        _productoService = productoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetAll()
    {
        try
        {
            var productos = await _productoService.GetAllAsync();
            return Ok(productos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los productos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDTO>> GetById(int id)
    {
        try
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound($"No se encontró el producto con ID {id}");
            }
            return Ok(producto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el producto con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("activos")]
    public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetActivos()
    {
        try
        {
            var productos = await _productoService.GetActivosAsync();
            return Ok(productos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los productos activos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ProductoDTO>> Create(CreateProductoDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producto = await _productoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el producto");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductoDTO>> Update(int id, UpdateProductoDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producto = await _productoService.UpdateAsync(id, dto);
            return Ok(producto);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el producto con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _productoService.DeleteAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el producto con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 