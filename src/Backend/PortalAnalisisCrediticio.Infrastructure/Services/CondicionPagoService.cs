using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.CondicionPago;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class CondicionPagoService : ICondicionPagoService
{
    private readonly ILogger<CondicionPagoService> _logger;
    private readonly IMemoryCache _cache;
    private readonly ApplicationDbContext _context;
    private const int CacheDurationMinutes = 60;

    public CondicionPagoService(
        ILogger<CondicionPagoService> logger,
        IMemoryCache cache,
        ApplicationDbContext context)
    {
        _logger = logger;
        _cache = cache;
        _context = context;
    }

    public async Task<IEnumerable<CondicionPagoDTO>> GetAllAsync()
    {
        var cacheKey = "CondicionPago_All";
        
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<CondicionPagoDTO>? condiciones))
        {
            condiciones = await _context.CondicionesPago
                .Select(cp => new CondicionPagoDTO
                {
                    Id = cp.Id,
                    SolicitudProductoId = cp.SolicitudProductoId,
                    PagoInicial = cp.PagoInicial,
                    CantidadCuotas = cp.CantidadCuotas,
                    TasaAplicada = cp.TasaAplicada,
                    Moneda = cp.Moneda,
                    FormaPago = cp.FormaPago,
                    MontoFinanciado = cp.MontoFinanciado,
                    MontoCuota = cp.MontoCuota,
                    Observaciones = cp.Observaciones,
                    FechaCreacion = cp.FechaCreacion,
                    FechaActualizacion = cp.FechaActualizacion
                })
                .ToListAsync();

            _cache.Set(cacheKey, condiciones, TimeSpan.FromMinutes(CacheDurationMinutes));
        }

        return condiciones ?? Enumerable.Empty<CondicionPagoDTO>();
    }

    public async Task<CondicionPagoDTO> GetByIdAsync(int id)
    {
        var cacheKey = $"CondicionPago_{id}";
        
        if (!_cache.TryGetValue(cacheKey, out CondicionPagoDTO? condicion))
        {
            var entity = await _context.CondicionesPago.FindAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Condición de pago con ID {Id} no encontrada", id);
                return null;
            }

            condicion = new CondicionPagoDTO
            {
                Id = entity.Id,
                SolicitudProductoId = entity.SolicitudProductoId,
                PagoInicial = entity.PagoInicial,
                CantidadCuotas = entity.CantidadCuotas,
                TasaAplicada = entity.TasaAplicada,
                Moneda = entity.Moneda,
                FormaPago = entity.FormaPago,
                MontoFinanciado = entity.MontoFinanciado,
                MontoCuota = entity.MontoCuota,
                Observaciones = entity.Observaciones,
                FechaCreacion = entity.FechaCreacion,
                FechaActualizacion = entity.FechaActualizacion
            };

            _cache.Set(cacheKey, condicion, TimeSpan.FromMinutes(CacheDurationMinutes));
        }

        return condicion;
    }

    public async Task<CondicionPagoDTO> GetBySolicitudIdAsync(int solicitudId)
    {
        var cacheKey = $"CondicionPago_Solicitud_{solicitudId}";
        
        if (!_cache.TryGetValue(cacheKey, out CondicionPagoDTO? condicion))
        {
            var entity = await _context.CondicionesPago
                .FirstOrDefaultAsync(cp => cp.SolicitudProductoId == solicitudId);

            if (entity == null)
            {
                _logger.LogWarning("Condición de pago para solicitud {SolicitudId} no encontrada", solicitudId);
                return null;
            }

            condicion = new CondicionPagoDTO
            {
                Id = entity.Id,
                SolicitudProductoId = entity.SolicitudProductoId,
                PagoInicial = entity.PagoInicial,
                CantidadCuotas = entity.CantidadCuotas,
                TasaAplicada = entity.TasaAplicada,
                Moneda = entity.Moneda,
                FormaPago = entity.FormaPago,
                MontoFinanciado = entity.MontoFinanciado,
                MontoCuota = entity.MontoCuota,
                Observaciones = entity.Observaciones,
                FechaCreacion = entity.FechaCreacion,
                FechaActualizacion = entity.FechaActualizacion
            };

            _cache.Set(cacheKey, condicion, TimeSpan.FromMinutes(CacheDurationMinutes));
        }

        return condicion;
    }

    public async Task<CondicionPagoDTO> CreateAsync(CreateCondicionPagoDTO condicionPagoDto)
    {
        var solicitud = await _context.SolicitudesProducto
            .FindAsync(condicionPagoDto.SolicitudProductoId);

        if (solicitud == null)
        {
            _logger.LogError("Solicitud de producto con ID {Id} no encontrada", condicionPagoDto.SolicitudProductoId);
            throw new ArgumentException("La solicitud de producto no existe");
        }

        var condicionPago = new CondicionPago
        {
            SolicitudProductoId = condicionPagoDto.SolicitudProductoId,
            PagoInicial = condicionPagoDto.PagoInicial,
            CantidadCuotas = condicionPagoDto.CantidadCuotas,
            TasaAplicada = condicionPagoDto.TasaAplicada,
            Moneda = condicionPagoDto.Moneda,
            FormaPago = condicionPagoDto.FormaPago,
            MontoFinanciado = condicionPagoDto.MontoFinanciado,
            MontoCuota = condicionPagoDto.MontoCuota,
            Observaciones = condicionPagoDto.Observaciones,
            FechaCreacion = DateTime.UtcNow
        };

        _context.CondicionesPago.Add(condicionPago);
        await _context.SaveChangesAsync();

        _cache.Remove("CondicionPago_All");
        _cache.Remove($"CondicionPago_Solicitud_{condicionPagoDto.SolicitudProductoId}");

        return new CondicionPagoDTO
        {
            Id = condicionPago.Id,
            SolicitudProductoId = condicionPago.SolicitudProductoId,
            PagoInicial = condicionPago.PagoInicial,
            CantidadCuotas = condicionPago.CantidadCuotas,
            TasaAplicada = condicionPago.TasaAplicada,
            Moneda = condicionPago.Moneda,
            FormaPago = condicionPago.FormaPago,
            MontoFinanciado = condicionPago.MontoFinanciado,
            MontoCuota = condicionPago.MontoCuota,
            Observaciones = condicionPago.Observaciones,
            FechaCreacion = condicionPago.FechaCreacion,
            FechaActualizacion = condicionPago.FechaActualizacion
        };
    }

    public async Task<CondicionPagoDTO> UpdateAsync(int id, UpdateCondicionPagoDTO condicionPagoDto)
    {
        var condicionPago = await _context.CondicionesPago.FindAsync(id);
        if (condicionPago == null)
        {
            _logger.LogWarning("Condición de pago con ID {Id} no encontrada", id);
            return null;
        }

        condicionPago.PagoInicial = condicionPagoDto.PagoInicial;
        condicionPago.CantidadCuotas = condicionPagoDto.CantidadCuotas;
        condicionPago.TasaAplicada = condicionPagoDto.TasaAplicada;
        condicionPago.Moneda = condicionPagoDto.Moneda;
        condicionPago.FormaPago = condicionPagoDto.FormaPago;
        condicionPago.MontoFinanciado = condicionPagoDto.MontoFinanciado;
        condicionPago.MontoCuota = condicionPagoDto.MontoCuota;
        condicionPago.Observaciones = condicionPagoDto.Observaciones;
        condicionPago.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _cache.Remove($"CondicionPago_{id}");
        _cache.Remove("CondicionPago_All");
        _cache.Remove($"CondicionPago_Solicitud_{condicionPago.SolicitudProductoId}");

        return new CondicionPagoDTO
        {
            Id = condicionPago.Id,
            SolicitudProductoId = condicionPago.SolicitudProductoId,
            PagoInicial = condicionPago.PagoInicial,
            CantidadCuotas = condicionPago.CantidadCuotas,
            TasaAplicada = condicionPago.TasaAplicada,
            Moneda = condicionPago.Moneda,
            FormaPago = condicionPago.FormaPago,
            MontoFinanciado = condicionPago.MontoFinanciado,
            MontoCuota = condicionPago.MontoCuota,
            Observaciones = condicionPago.Observaciones,
            FechaCreacion = condicionPago.FechaCreacion,
            FechaActualizacion = condicionPago.FechaActualizacion
        };
    }

    public async Task DeleteAsync(int id)
    {
        var condicionPago = await _context.CondicionesPago.FindAsync(id);
        if (condicionPago == null)
        {
            _logger.LogWarning("Condición de pago con ID {Id} no encontrada", id);
            return;
        }

        _context.CondicionesPago.Remove(condicionPago);
        await _context.SaveChangesAsync();

        _cache.Remove($"CondicionPago_{id}");
        _cache.Remove("CondicionPago_All");
        _cache.Remove($"CondicionPago_Solicitud_{condicionPago.SolicitudProductoId}");
    }
} 