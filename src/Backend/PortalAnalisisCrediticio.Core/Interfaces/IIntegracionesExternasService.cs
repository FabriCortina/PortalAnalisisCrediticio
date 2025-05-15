using PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface IIntegracionesExternasService
{
    Task<InformeNosisDTO> GetInformeNosisAsync(int clienteId);
    Task<InformeVerazDTO> GetInformeVerazAsync(int clienteId);
    Task<InformeInfoexpertoDTO> GetInformeInfoexpertoAsync(int clienteId);
    Task<InformeBCRADTO> GetInformeBCRAAsync(int clienteId);
    Task<InformeAFIPDTO> GetInformeAFIPAsync(int clienteId);
    Task<InformePublicoDTO> GetInformePublicoAsync(int clienteId);
} 