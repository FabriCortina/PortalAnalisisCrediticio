using PortalAnalisisCrediticio.Shared.DTOs.AnalisisRiesgo;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface IAnalisisRiesgoService
{
    Task<InformeRiesgoDTO> RealizarAnalisisRiesgoAsync(int clienteId);
    Task<InformeRiesgoDTO> GetInformeRiesgoAsync(int clienteId);
    Task<byte[]> ExportarInformePDFAsync(int clienteId);
} 