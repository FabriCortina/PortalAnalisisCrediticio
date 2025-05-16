using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Producto;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de productos financieros
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;
    private readonly ILogger<ProductosController> _logger;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="productoService">Servicio de productos</param>
    /// <param name="logger">Logger para el controlador</param>
    public ProductosController(
        IProductoService productoService,
        ILogger<ProductosController> logger)
    {
        _productoService = productoService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los productos financieros
    /// </summary>
    /// <returns>Lista de productos</returns>
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

    /// <summary>
    /// Obtiene un producto específico por su ID
    /// </summary>
    /// <param name="id">ID del producto</param>
    /// <returns>Datos del producto</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDTO>> GetById(int id)
    {
        try
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el producto con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea un nuevo producto financiero
    /// </summary>
    /// <param name="productoDto">Datos del producto a crear</param>
    /// <returns>Datos del producto creado</returns>
    [HttpPost]
    public async Task<ActionResult<ProductoDTO>> Create(ProductoDTO productoDto)
    {
        try
        {
            var producto = await _productoService.CreateAsync(productoDto);
            return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el producto");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza un producto existente
    /// </summary>
    /// <param name="id">ID del producto</param>
    /// <param name="productoDto">Datos actualizados del producto</param>
    /// <returns>Sin contenido</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductoDTO productoDto)
    {
        try
        {
            await _productoService.UpdateAsync(id, productoDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el producto con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina un producto
    /// </summary>
    /// <param name="id">ID del producto</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _productoService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el producto con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene los productos activos
    /// </summary>
    /// <returns>Lista de productos activos</returns>
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

    /// <summary>
    /// Obtiene los productos por tipo
    /// </summary>
    /// <param name="tipo">Tipo de producto</param>
    /// <returns>Lista de productos del tipo especificado</returns>
    [HttpGet("tipo/{tipo}")]
    public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetByTipo(string tipo)
    {
        try
        {
            var productos = await _productoService.GetByTipoAsync(tipo);
            return Ok(productos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los productos de tipo {Tipo}", tipo);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 