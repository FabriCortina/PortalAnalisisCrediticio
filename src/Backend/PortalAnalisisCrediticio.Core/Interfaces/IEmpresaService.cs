using PortalAnalisisCrediticio.Shared.DTOs;
using PortalAnalisisCrediticio.Shared.DTOs.Empresa;
using ClienteDTO = PortalAnalisisCrediticio.Shared.DTOs.Cliente.ClienteDTO;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface IEmpresaService
{
    Task<IEnumerable<EmpresaDTO>> GetAllAsync();
    Task<EmpresaDTO> GetByIdAsync(int id);
    Task<EmpresaDTO> CreateAsync(CreateEmpresaDTO empresaDto);
    Task<EmpresaDTO> UpdateAsync(int id, UpdateEmpresaDTO empresaDto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<EmpresaDTO>> SearchAsync(string searchTerm);
    Task<EmpresaDTO> GetByRucAsync(string ruc);
    Task<ClienteEmpresaDTO> AsociarClienteAsync(int empresaId, int clienteId);
    Task<bool> DesasociarClienteAsync(int empresaId, int clienteId);
    Task<IEnumerable<ClienteDTO>> GetClientesAsync(int empresaId);
} 