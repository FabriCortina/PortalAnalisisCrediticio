using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.Credito;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class CreditoService : ICreditoService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CreditoService> _logger;

    public CreditoService(ApplicationDbContext context, ILogger<CreditoService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CreditoDTO> GetByIdAsync(int id)
    {
        var credito = await _context.Creditos
            .Include(c => c.Pagos)
            .FirstOrDefaultAsync(c => c.Id == id);
        return credito == null ? null : MapToDTO(credito);
    }

    public async Task<IEnumerable<CreditoDTO>> GetByClienteIdAsync(int clienteId)
    {
        var creditos = await _context.Creditos
            .Include(c => c.Pagos)
            .Where(c => c.ClienteId == clienteId)
            .ToListAsync();
        return creditos.Select(MapToDTO);
    }

    public async Task<CreditoDTO> CreateAsync(CreateCreditoDTO creditoDto)
    {
        var credito = new Credito
        {
            ClienteId = creditoDto.ClienteId,
            SolicitudProductoId = creditoDto.SolicitudProductoId,
            FechaOtorgamiento = creditoDto.FechaOtorgamiento,
            MontoOtorgado = creditoDto.MontoOtorgado,
            CantidadCuotas = creditoDto.CantidadCuotas,
            Estado = creditoDto.Estado,
            TasaInteres = creditoDto.TasaInteres,
            SaldoPendiente = creditoDto.MontoOtorgado
        };
        _context.Creditos.Add(credito);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(credito.Id);
    }

    public async Task<CreditoDTO> UpdateAsync(int id, UpdateCreditoDTO creditoDto)
    {
        var credito = await _context.Creditos.FindAsync(id);
        if (credito == null) return null;
        credito.Estado = creditoDto.Estado;
        credito.TasaInteres = creditoDto.TasaInteres;
        credito.FechaUltimoPago = creditoDto.FechaUltimoPago;
        credito.SaldoPendiente = creditoDto.SaldoPendiente;
        await _context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var credito = await _context.Creditos.FindAsync(id);
        if (credito != null)
        {
            _context.Creditos.Remove(credito);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<PagoCreditoDTO> RegistrarPagoAsync(CreatePagoCreditoDTO pagoDto)
    {
        var credito = await _context.Creditos.Include(c => c.Pagos).FirstOrDefaultAsync(c => c.Id == pagoDto.CreditoId);
        if (credito == null) return null;
        var pago = new PagoCredito
        {
            CreditoId = pagoDto.CreditoId,
            FechaPago = pagoDto.FechaPago,
            MontoPagado = pagoDto.MontoPagado,
            NumeroCuota = pagoDto.NumeroCuota,
            VencimientoCuota = pagoDto.VencimientoCuota,
            EstadoCuota = pagoDto.EstadoCuota
        };
        _context.PagosCredito.Add(pago);
        // Actualizar saldo y fecha último pago
        credito.SaldoPendiente -= pagoDto.MontoPagado;
        credito.FechaUltimoPago = pagoDto.FechaPago;
        if (credito.SaldoPendiente <= 0)
            credito.Estado = "Cancelado";
        await _context.SaveChangesAsync();
        return MapToDTO(pago);
    }

    public async Task<IEnumerable<PagoCreditoDTO>> GetPagosByCreditoIdAsync(int creditoId)
    {
        var pagos = await _context.PagosCredito
            .Where(p => p.CreditoId == creditoId)
            .OrderBy(p => p.NumeroCuota)
            .ToListAsync();
        return pagos.Select(MapToDTO);
    }

    public async Task<IEnumerable<CreditoDTO>> GetHistorialCreditosPorClienteAsync(int clienteId)
    {
        var creditos = await _context.Creditos
            .Include(c => c.Pagos)
            .Where(c => c.ClienteId == clienteId)
            .OrderByDescending(c => c.FechaOtorgamiento)
            .ToListAsync();
        return creditos.Select(MapToDTO);
    }

    // Mapeos
    private CreditoDTO MapToDTO(Credito c)
    {
        return new CreditoDTO
        {
            Id = c.Id,
            ClienteId = c.ClienteId,
            SolicitudProductoId = c.SolicitudProductoId,
            FechaOtorgamiento = c.FechaOtorgamiento,
            MontoOtorgado = c.MontoOtorgado,
            CantidadCuotas = c.CantidadCuotas,
            Estado = c.Estado,
            TasaInteres = c.TasaInteres,
            FechaUltimoPago = c.FechaUltimoPago,
            SaldoPendiente = c.SaldoPendiente,
            Pagos = c.Pagos?.Select(MapToDTO).ToList() ?? new List<PagoCreditoDTO>()
        };
    }
    private PagoCreditoDTO MapToDTO(PagoCredito p)
    {
        return new PagoCreditoDTO
        {
            Id = p.Id,
            CreditoId = p.CreditoId,
            FechaPago = p.FechaPago,
            MontoPagado = p.MontoPagado,
            NumeroCuota = p.NumeroCuota,
            VencimientoCuota = p.VencimientoCuota,
            EstadoCuota = p.EstadoCuota
        };
    }
} 