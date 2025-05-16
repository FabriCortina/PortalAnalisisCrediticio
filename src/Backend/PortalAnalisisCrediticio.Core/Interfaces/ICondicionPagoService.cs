using PortalAnalisisCrediticio.Shared.DTOs.CondicionPago;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface ICondicionPagoService
{
    Task<IEnumerable<CondicionPagoDTO>> GetAllAsync();
    Task<CondicionPagoDTO> GetByIdAsync(int id);
    Task<CondicionPagoDTO> GetBySolicitudIdAsync(int solicitudId);
    Task<CondicionPagoDTO> CreateAsync(CreateCondicionPagoDTO condicionPagoDto);
    Task<CondicionPagoDTO> UpdateAsync(int id, UpdateCondicionPagoDTO condicionPagoDto);
    Task DeleteAsync(int id);
} 