using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.SolicitudProducto;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class SolicitudProductoService : ISolicitudProductoService
{
    private readonly ILogger<SolicitudProductoService> _logger;
    private readonly IMemoryCache _cache;
    private readonly ApplicationDbContext _context;
    private const int CacheDurationMinutes = 60;

    public SolicitudProductoService(
        ILogger<SolicitudProductoService> logger,
        IMemoryCache cache,
        ApplicationDbContext context)
    {
        _logger = logger;
        _cache = cache;
        _context = context;
    }

    public async Task<IEnumerable<SolicitudProductoDTO>> GetAllAsync()
    {
        var cacheKey = "SolicitudesProducto_All";
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<SolicitudProductoDTO> solicitudes))
        {
            solicitudes = await _context.SolicitudesProducto
                .Include(s => s.Cliente)
                .Include(s => s.ProductoSolicitudes)
                    .ThenInclude(ps => ps.Producto)
                .Select(s => new SolicitudProductoDTO
                {
                    Id = s.Id,
                    ClienteId = s.ClienteId,
                    ClienteNombre = s.Cliente.Nombre,
                    FechaSolicitud = s.FechaSolicitud,
                    MontoTotal = s.MontoTotal,
                    Estado = s.Estado,
                    PagoInicial = s.PagoInicial,
                    PorcentajeFinanciacion = s.PorcentajeFinanciacion,
                    CantidadCuotas = s.CantidadCuotas,
                    MontoFinanciado = s.MontoFinanciado,
                    MontoCuota = s.MontoCuota,
                    Observaciones = s.Observaciones,
                    Productos = s.ProductoSolicitudes.Select(ps => new ProductoSolicitudDTO
                    {
                        Id = ps.Id,
                        ProductoId = ps.ProductoId,
                        ProductoNombre = ps.Producto.Nombre,
                        ProductoCodigoInterno = ps.Producto.CodigoInterno,
                        PrecioUnitario = ps.Producto.PrecioUnitario,
                        Moneda = ps.Producto.Moneda,
                        Cantidad = ps.Cantidad,
                        Subtotal = ps.Subtotal
                    }).ToList()
                })
                .ToListAsync();

            _cache.Set(cacheKey, solicitudes, TimeSpan.FromMinutes(CacheDurationMinutes));
        }

        return solicitudes;
    }

    public async Task<SolicitudProductoDTO> GetByIdAsync(int id)
    {
        var cacheKey = $"SolicitudProducto_{id}";
        if (!_cache.TryGetValue(cacheKey, out SolicitudProductoDTO solicitud))
        {
            solicitud = await _context.SolicitudesProducto
                .Include(s => s.Cliente)
                .Include(s => s.ProductoSolicitudes)
                    .ThenInclude(ps => ps.Producto)
                .Where(s => s.Id == id)
                .Select(s => new SolicitudProductoDTO
                {
                    Id = s.Id,
                    ClienteId = s.ClienteId,
                    ClienteNombre = s.Cliente.Nombre,
                    FechaSolicitud = s.FechaSolicitud,
                    MontoTotal = s.MontoTotal,
                    Estado = s.Estado,
                    PagoInicial = s.PagoInicial,
                    PorcentajeFinanciacion = s.PorcentajeFinanciacion,
                    CantidadCuotas = s.CantidadCuotas,
                    MontoFinanciado = s.MontoFinanciado,
                    MontoCuota = s.MontoCuota,
                    Observaciones = s.Observaciones,
                    Productos = s.ProductoSolicitudes.Select(ps => new ProductoSolicitudDTO
                    {
                        Id = ps.Id,
                        ProductoId = ps.ProductoId,
                        ProductoNombre = ps.Producto.Nombre,
                        ProductoCodigoInterno = ps.Producto.CodigoInterno,
                        PrecioUnitario = ps.Producto.PrecioUnitario,
                        Moneda = ps.Producto.Moneda,
                        Cantidad = ps.Cantidad,
                        Subtotal = ps.Subtotal
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (solicitud != null)
            {
                _cache.Set(cacheKey, solicitud, TimeSpan.FromMinutes(CacheDurationMinutes));
            }
        }

        return solicitud;
    }

    public async Task<IEnumerable<SolicitudProductoDTO>> GetByClienteIdAsync(int clienteId)
    {
        var cacheKey = $"SolicitudesProducto_Cliente_{clienteId}";
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<SolicitudProductoDTO> solicitudes))
        {
            solicitudes = await _context.SolicitudesProducto
                .Include(s => s.Cliente)
                .Include(s => s.ProductoSolicitudes)
                    .ThenInclude(ps => ps.Producto)
                .Where(s => s.ClienteId == clienteId)
                .Select(s => new SolicitudProductoDTO
                {
                    Id = s.Id,
                    ClienteId = s.ClienteId,
                    ClienteNombre = s.Cliente.Nombre,
                    FechaSolicitud = s.FechaSolicitud,
                    MontoTotal = s.MontoTotal,
                    Estado = s.Estado,
                    PagoInicial = s.PagoInicial,
                    PorcentajeFinanciacion = s.PorcentajeFinanciacion,
                    CantidadCuotas = s.CantidadCuotas,
                    MontoFinanciado = s.MontoFinanciado,
                    MontoCuota = s.MontoCuota,
                    Observaciones = s.Observaciones,
                    Productos = s.ProductoSolicitudes.Select(ps => new ProductoSolicitudDTO
                    {
                        Id = ps.Id,
                        ProductoId = ps.ProductoId,
                        ProductoNombre = ps.Producto.Nombre,
                        ProductoCodigoInterno = ps.Producto.CodigoInterno,
                        PrecioUnitario = ps.Producto.PrecioUnitario,
                        Moneda = ps.Producto.Moneda,
                        Cantidad = ps.Cantidad,
                        Subtotal = ps.Subtotal
                    }).ToList()
                })
                .ToListAsync();

            _cache.Set(cacheKey, solicitudes, TimeSpan.FromMinutes(CacheDurationMinutes));
        }

        return solicitudes;
    }

    public async Task<SolicitudProductoDTO> CreateAsync(CreateSolicitudProductoDTO dto)
    {
        try
        {
            var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
            if (cliente == null)
            {
                throw new ArgumentException($"No se encontró el cliente con ID {dto.ClienteId}");
            }

            var solicitud = new SolicitudProducto
            {
                ClienteId = dto.ClienteId,
                FechaSolicitud = dto.FechaSolicitud,
                MontoTotal = dto.MontoTotal,
                Estado = dto.Estado,
                PagoInicial = dto.PagoInicial,
                PorcentajeFinanciacion = dto.PorcentajeFinanciacion,
                CantidadCuotas = dto.CantidadCuotas,
                MontoFinanciado = dto.MontoFinanciado,
                MontoCuota = dto.MontoCuota,
                Observaciones = dto.Observaciones,
                ProductoSolicitudes = dto.Productos.Select(p => new ProductoSolicitud
                {
                    ProductoId = p.ProductoId,
                    Cantidad = p.Cantidad,
                    Subtotal = p.Subtotal
                }).ToList()
            };

            _context.SolicitudesProducto.Add(solicitud);
            await _context.SaveChangesAsync();

            InvalidateCache();

            return await GetByIdAsync(solicitud.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la solicitud de producto");
            throw;
        }
    }

    public async Task<SolicitudProductoDTO> UpdateAsync(int id, CreateSolicitudProductoDTO dto)
    {
        try
        {
            var solicitud = await _context.SolicitudesProducto
                .Include(s => s.ProductoSolicitudes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (solicitud == null)
            {
                throw new ArgumentException($"No se encontró la solicitud con ID {id}");
            }

            var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
            if (cliente == null)
            {
                throw new ArgumentException($"No se encontró el cliente con ID {dto.ClienteId}");
            }

            solicitud.ClienteId = dto.ClienteId;
            solicitud.FechaSolicitud = dto.FechaSolicitud;
            solicitud.MontoTotal = dto.MontoTotal;
            solicitud.Estado = dto.Estado;
            solicitud.PagoInicial = dto.PagoInicial;
            solicitud.PorcentajeFinanciacion = dto.PorcentajeFinanciacion;
            solicitud.CantidadCuotas = dto.CantidadCuotas;
            solicitud.MontoFinanciado = dto.MontoFinanciado;
            solicitud.MontoCuota = dto.MontoCuota;
            solicitud.Observaciones = dto.Observaciones;

            // Actualizar productos
            _context.ProductoSolicitudes.RemoveRange(solicitud.ProductoSolicitudes);
            solicitud.ProductoSolicitudes = dto.Productos.Select(p => new ProductoSolicitud
            {
                ProductoId = p.ProductoId,
                Cantidad = p.Cantidad,
                Subtotal = p.Subtotal
            }).ToList();

            await _context.SaveChangesAsync();

            InvalidateCache();

            return await GetByIdAsync(solicitud.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la solicitud de producto");
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var solicitud = await _context.SolicitudesProducto
                .Include(s => s.ProductoSolicitudes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (solicitud == null)
            {
                throw new ArgumentException($"No se encontró la solicitud con ID {id}");
            }

            _context.ProductoSolicitudes.RemoveRange(solicitud.ProductoSolicitudes);
            _context.SolicitudesProducto.Remove(solicitud);
            await _context.SaveChangesAsync();

            InvalidateCache();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la solicitud de producto");
            throw;
        }
    }

    public async Task<SolicitudProductoDTO> CambiarEstadoAsync(int id, string nuevoEstado)
    {
        try
        {
            var solicitud = await _context.SolicitudesProducto.FindAsync(id);
            if (solicitud == null)
            {
                throw new ArgumentException($"No se encontró la solicitud con ID {id}");
            }

            solicitud.Estado = nuevoEstado;
            await _context.SaveChangesAsync();

            InvalidateCache();

            return await GetByIdAsync(solicitud.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar el estado de la solicitud de producto");
            throw;
        }
    }

    private void InvalidateCache()
    {
        _cache.Remove("SolicitudesProducto_All");
        _cache.Remove("SolicitudesProducto_Cliente_*");
        _cache.Remove("SolicitudProducto_*");
    }
} 