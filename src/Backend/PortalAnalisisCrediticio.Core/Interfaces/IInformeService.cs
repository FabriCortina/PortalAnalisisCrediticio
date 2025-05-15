using PortalAnalisisCrediticio.Shared.DTOs.Informes;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface IInformeService
{
    Task<InformeDTO> GenerarInformeAsync(int clienteId);
    Task<byte[]> ExportarInformePDFAsync(int clienteId);
    Task<IEnumerable<InformeHistoricoDTO>> GetHistorialInformesAsync(int clienteId);
} 