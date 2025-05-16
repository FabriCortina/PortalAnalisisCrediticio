using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Shared.DTOs.Cliente;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<ClienteDTO>> GetAllAsync();
    Task<IEnumerable<ClienteDTO>> GetAllWithDetailsAsync();
    Task<ClienteDTO> GetByIdAsync(int id);
    Task<ClienteDTO> GetByIdWithDetailsAsync(int id);
    Task<ClienteDTO> CreateAsync(CreateClienteDTO clienteDto);
    Task<ClienteDTO> CreateWithDetailsAsync(ClienteDTO clienteDto);
    Task<ClienteDTO> UpdateAsync(int id, UpdateClienteDTO clienteDto);
    Task<ClienteDTO> UpdateWithDetailsAsync(int id, ClienteDTO clienteDto);
    Task DeleteAsync(int id);
    Task<IEnumerable<ClienteDTO>> SearchAsync(string searchTerm);
    Task<ClienteDTO> GetByEmailAsync(string email);
    Task<ClienteDTO> GetByDocumentoAsync(string documento);
    Task<ClienteDTO> ImportarDesdeExcelAsync(Stream fileStream);
} 