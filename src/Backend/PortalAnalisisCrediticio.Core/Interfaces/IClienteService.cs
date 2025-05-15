using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<ClienteDTO>> GetAllAsync();
    Task<IEnumerable<ClienteDTO>> GetAllWithDetailsAsync();
    Task<ClienteDTO> GetByIdAsync(int id);
    Task<ClienteDTO> GetByIdWithDetailsAsync(int id);
    Task<ClienteDTO> CreateAsync(ClienteDTO clienteDto);
    Task<ClienteDTO> CreateWithDetailsAsync(ClienteDTO clienteDto);
    Task<ClienteDTO> UpdateAsync(int id, ClienteDTO clienteDto);
    Task<ClienteDTO> UpdateWithDetailsAsync(int id, ClienteDTO clienteDto);
    Task<bool> DeleteAsync(int id);
    Task<ClienteDTO> GetLegajoDigitalAsync(int id);
    Task<ClienteDTO> ImportarDesdeExcelAsync(Stream fileStream);
} 