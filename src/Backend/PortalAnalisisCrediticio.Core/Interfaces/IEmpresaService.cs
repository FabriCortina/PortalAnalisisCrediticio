using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface IEmpresaService
{
    Task<IEnumerable<EmpresaDTO>> GetAllAsync();
    Task<EmpresaDTO> GetByIdAsync(int id);
    Task<EmpresaDTO> CreateAsync(EmpresaDTO empresaDto);
    Task<EmpresaDTO> UpdateAsync(int id, EmpresaDTO empresaDto);
    Task<bool> DeleteAsync(int id);
    Task<ClienteEmpresaDTO> AsociarClienteAsync(int empresaId, int clienteId);
    Task<bool> DesasociarClienteAsync(int empresaId, int clienteId);
    Task<IEnumerable<ClienteDTO>> GetClientesAsync(int empresaId);
} 