using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface ISolicitudProductoService
{
    Task<IEnumerable<SolicitudProductoDTO>> GetAllAsync();
    Task<IEnumerable<SolicitudProductoDTO>> GetByClienteIdAsync(int clienteId);
    Task<SolicitudProductoDTO> GetByIdAsync(int id);
    Task<SolicitudProductoDTO> CreateAsync(CreateSolicitudProductoDTO solicitudDto);
    Task<SolicitudProductoDTO> UpdateAsync(int id, UpdateSolicitudProductoDTO solicitudDto);
    Task DeleteAsync(int id);
    Task<IEnumerable<SolicitudProductoDTO>> GetPendientesAsync();
    Task<SolicitudProductoDTO> CambiarEstadoAsync(int id, string nuevoEstado);
} 