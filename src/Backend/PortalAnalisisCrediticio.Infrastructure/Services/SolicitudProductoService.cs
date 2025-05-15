using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class SolicitudProductoService : BaseService<SolicitudProducto, SolicitudProductoDTO>, ISolicitudProductoService
{
    public SolicitudProductoService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<IEnumerable<SolicitudProductoDTO>> GetByClienteIdAsync(int clienteId)
    {
        var solicitudes = await _dbSet
            .Where(s => s.ClienteId == clienteId)
            .OrderByDescending(s => s.FechaSolicitud)
            .ToListAsync();

        return _mapper.Map<IEnumerable<SolicitudProductoDTO>>(solicitudes);
    }

    public async Task<SolicitudProductoDTO> CreateAsync(CreateSolicitudProductoDTO solicitudDto)
    {
        var solicitud = _mapper.Map<SolicitudProducto>(solicitudDto);
        solicitud.FechaSolicitud = DateTime.Now;
        solicitud.Estado = "Pendiente";

        // Validar que el cliente exista
        var cliente = await _context.Clientes.FindAsync(solicitud.ClienteId);
        if (cliente == null)
            throw new ArgumentException("El cliente no existe");

        // Validar que el pago inicial no sea mayor al monto total
        if (solicitud.PagoInicial > solicitud.MontoTotal)
            throw new ArgumentException("El pago inicial no puede ser mayor al monto total");

        // Validar que el porcentaje de financiación sea coherente con el pago inicial
        var montoFinanciado = solicitud.MontoTotal - solicitud.PagoInicial;
        var porcentajeCalculado = (montoFinanciado / solicitud.MontoTotal) * 100;
        if (Math.Abs(porcentajeCalculado - solicitud.PorcentajeFinanciacion) > 0.01m)
            throw new ArgumentException("El porcentaje de financiación no coincide con el pago inicial");

        _dbSet.Add(solicitud);
        await _context.SaveChangesAsync();

        return _mapper.Map<SolicitudProductoDTO>(solicitud);
    }

    public async Task<SolicitudProductoDTO> UpdateAsync(int id, UpdateSolicitudProductoDTO solicitudDto)
    {
        var solicitud = await _dbSet.FindAsync(id);
        if (solicitud == null)
            return null;

        _mapper.Map(solicitudDto, solicitud);

        // Validar que el pago inicial no sea mayor al monto total
        if (solicitud.PagoInicial > solicitud.MontoTotal)
            throw new ArgumentException("El pago inicial no puede ser mayor al monto total");

        // Validar que el porcentaje de financiación sea coherente con el pago inicial
        var montoFinanciado = solicitud.MontoTotal - solicitud.PagoInicial;
        var porcentajeCalculado = (montoFinanciado / solicitud.MontoTotal) * 100;
        if (Math.Abs(porcentajeCalculado - solicitud.PorcentajeFinanciacion) > 0.01m)
            throw new ArgumentException("El porcentaje de financiación no coincide con el pago inicial");

        await _context.SaveChangesAsync();

        return _mapper.Map<SolicitudProductoDTO>(solicitud);
    }

    public async Task<IEnumerable<SolicitudProductoDTO>> GetPendientesAsync()
    {
        var solicitudes = await _dbSet
            .Where(s => s.Estado == "Pendiente")
            .OrderByDescending(s => s.FechaSolicitud)
            .ToListAsync();

        return _mapper.Map<IEnumerable<SolicitudProductoDTO>>(solicitudes);
    }

    public async Task<SolicitudProductoDTO> CambiarEstadoAsync(int id, string nuevoEstado)
    {
        var solicitud = await _dbSet.FindAsync(id);
        if (solicitud == null)
            return null;

        solicitud.Estado = nuevoEstado;
        await _context.SaveChangesAsync();

        return _mapper.Map<SolicitudProductoDTO>(solicitud);
    }
} 