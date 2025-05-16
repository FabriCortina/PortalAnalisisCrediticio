using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.Producto;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class ProductoService : BaseService<Producto, ProductoDTO>, IProductoService
{
    private readonly ILogger<ProductoService> _logger;
    private readonly IMemoryCache _cache;
    private const int CACHE_DURATION_MINUTES = 60;

    public ProductoService(
        ApplicationDbContext context, 
        IMapper mapper,
        ILogger<ProductoService> logger,
        IMemoryCache cache) : base(context, mapper)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<IEnumerable<ProductoDTO>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo todos los productos");

            // Verificar caché
            var cacheKey = "Productos_All";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<ProductoDTO> productosCache))
            {
                _logger.LogInformation("Retornando productos desde caché");
                return productosCache;
            }

            var productos = await _dbSet.ToListAsync();
            var dtos = _mapper.Map<IEnumerable<ProductoDTO>>(productos);

            // Guardar en caché
            _cache.Set(cacheKey, dtos, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Se obtuvieron {dtos.Count()} productos exitosamente");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos");
            throw new Exception("Error al obtener productos", ex);
        }
    }

    public async Task<ProductoDTO> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Obteniendo producto {id}");

            // Verificar caché
            var cacheKey = $"Producto_{id}";
            if (_cache.TryGetValue(cacheKey, out ProductoDTO productoCache))
            {
                _logger.LogInformation($"Retornando producto {id} desde caché");
                return productoCache;
            }

            var producto = await _dbSet.FindAsync(id);
            if (producto == null)
            {
                _logger.LogWarning($"Producto {id} no encontrado");
                return null;
            }

            var dto = _mapper.Map<ProductoDTO>(producto);

            // Guardar en caché
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Producto {id} obtenido exitosamente");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener producto {id}");
            throw new Exception($"Error al obtener producto {id}", ex);
        }
    }

    public async Task<IEnumerable<ProductoDTO>> GetActivosAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo productos activos");

            // Verificar caché
            var cacheKey = "Productos_Activos";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<ProductoDTO> productosCache))
            {
                _logger.LogInformation("Retornando productos activos desde caché");
                return productosCache;
            }

            var productos = await _dbSet.Where(p => p.Activo).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<ProductoDTO>>(productos);

            // Guardar en caché
            _cache.Set(cacheKey, dtos, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Se obtuvieron {dtos.Count()} productos activos exitosamente");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos activos");
            throw new Exception("Error al obtener productos activos", ex);
        }
    }

    public async Task<ProductoDTO> CreateAsync(CreateProductoDTO productoDto)
    {
        try
        {
            _logger.LogInformation("Creando nuevo producto");

            // Validar código interno único
            var codigoExistente = await _dbSet.AnyAsync(p => p.CodigoInterno == productoDto.CodigoInterno);
            if (codigoExistente)
            {
                throw new ArgumentException($"Ya existe un producto con el código interno {productoDto.CodigoInterno}");
            }

            var producto = _mapper.Map<Producto>(productoDto);
            producto.FechaCreacion = DateTime.UtcNow;

            await _dbSet.AddAsync(producto);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<ProductoDTO>(producto);

            // Invalidar caché
            _cache.Remove("Productos_All");
            _cache.Remove("Productos_Activos");

            _logger.LogInformation($"Producto {producto.Id} creado exitosamente");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear producto");
            throw new Exception("Error al crear producto", ex);
        }
    }

    public async Task<ProductoDTO> UpdateAsync(int id, UpdateProductoDTO productoDto)
    {
        try
        {
            _logger.LogInformation($"Actualizando producto {id}");

            var producto = await _dbSet.FindAsync(id);
            if (producto == null)
            {
                _logger.LogWarning($"Producto {id} no encontrado");
                return null;
            }

            // Validar código interno único si cambió
            if (producto.CodigoInterno != productoDto.CodigoInterno)
            {
                var codigoExistente = await _dbSet.AnyAsync(p => p.CodigoInterno == productoDto.CodigoInterno);
                if (codigoExistente)
                {
                    throw new ArgumentException($"Ya existe un producto con el código interno {productoDto.CodigoInterno}");
                }
            }

            _mapper.Map(productoDto, producto);
            producto.FechaActualizacion = DateTime.UtcNow;

            _dbSet.Update(producto);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<ProductoDTO>(producto);

            // Invalidar caché
            _cache.Remove($"Producto_{id}");
            _cache.Remove("Productos_All");
            _cache.Remove("Productos_Activos");

            _logger.LogInformation($"Producto {id} actualizado exitosamente");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar producto {id}");
            throw new Exception($"Error al actualizar producto {id}", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Eliminando producto {id}");

            var producto = await _dbSet.FindAsync(id);
            if (producto == null)
            {
                _logger.LogWarning($"Producto {id} no encontrado");
                return false;
            }

            _dbSet.Remove(producto);
            await _context.SaveChangesAsync();

            // Invalidar caché
            _cache.Remove($"Producto_{id}");
            _cache.Remove("Productos_All");
            _cache.Remove("Productos_Activos");

            _logger.LogInformation($"Producto {id} eliminado exitosamente");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar producto {id}");
            throw new Exception($"Error al eliminar producto {id}", ex);
        }
    }
} 