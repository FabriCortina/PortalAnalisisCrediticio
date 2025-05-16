using PortalAnalisisCrediticio.Shared.DTOs.InformeCrediticio;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface IInformeCrediticioService
{
    Task<InformeCrediticioDTO> GetByIdAsync(int id);
    Task<IEnumerable<InformeCrediticioDTO>> GetByClienteIdAsync(int clienteId);
    Task<InformeCrediticioDTO> CreateAsync(CreateInformeCrediticioDTO informeDto);
    Task<InformeCrediticioDTO> UpdateAsync(int id, UpdateInformeCrediticioDTO informeDto);
    Task DeleteAsync(int id);
    Task<byte[]> ExportarPDFAsync(int id);
    Task<string> GenerarVistaWebAsync(int id);
} 