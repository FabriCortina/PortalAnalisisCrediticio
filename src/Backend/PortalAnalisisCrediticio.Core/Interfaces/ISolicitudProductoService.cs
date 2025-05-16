using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Shared.DTOs.Producto;
using PortalAnalisisCrediticio.Shared.DTOs.SolicitudProducto;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface ISolicitudProductoService
{
    Task<IEnumerable<SolicitudProductoDTO>> GetAllAsync();
    Task<SolicitudProductoDTO> GetByIdAsync(int id);
    Task<IEnumerable<SolicitudProductoDTO>> GetByClienteIdAsync(int clienteId);
    Task<SolicitudProductoDTO> CreateAsync(CreateSolicitudProductoDTO dto);
    Task<SolicitudProductoDTO> UpdateAsync(int id, CreateSolicitudProductoDTO dto);
    Task DeleteAsync(int id);
    Task<SolicitudProductoDTO> CambiarEstadoAsync(int id, string nuevoEstado);
    Task<SolicitudProductoDTO> GetByNumeroSolicitudAsync(string numeroSolicitud);
} 