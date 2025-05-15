using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface IInformacionFinancieraService
{
    Task<IEnumerable<InformacionFinancieraDTO>> GetAllAsync();
    Task<InformacionFinancieraDTO> GetByIdAsync(int id);
    Task<InformacionFinancieraDTO> CreateAsync(int clienteId, InformacionFinancieraDTO informacionDto);
    Task<InformacionFinancieraDTO> UpdateAsync(int clienteId, InformacionFinancieraDTO informacionDto);
    Task DeleteAsync(int clienteId);
    Task<InformacionFinancieraDTO> GetByClienteIdAsync(int clienteId);
    Task<InformacionFinancieraDTO> ImportarDesdeExcelAsync(Stream fileStream);
    Task<EstadoFinancieroDTO> AddEstadoFinancieroAsync(int clienteId, EstadoFinancieroDTO estadoDto);
    Task<FlujoCajaProyectadoDTO> AddFlujoCajaAsync(int clienteId, FlujoCajaProyectadoDTO flujoDto);
    Task<DeudaDTO> AddDeudaAsync(int clienteId, DeudaDTO deudaDto);
} 