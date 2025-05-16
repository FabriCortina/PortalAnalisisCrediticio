using PortalAnalisisCrediticio.Shared.DTOs.Producto;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<ProductoDTO>> GetAllAsync();
    Task<ProductoDTO> GetByIdAsync(int id);
    Task<IEnumerable<ProductoDTO>> GetActivosAsync();
    Task<ProductoDTO> CreateAsync(CreateProductoDTO dto);
    Task<ProductoDTO> UpdateAsync(int id, UpdateProductoDTO dto);
    Task DeleteAsync(int id);
} 