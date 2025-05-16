using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Core.Interfaces
{
    public interface IAnalisisService
    {
        Task<MetricasAnalisisDto> GetMetricasAsync(int periodo);
        Task<DistribucionRiesgosDto> GetDistribucionRiesgosAsync(int periodo);
        Task<TendenciasCreditoDto> GetTendenciasCreditoAsync(int periodo);
        Task<IEnumerable<UltimoAnalisisDto>> GetUltimosAnalisisAsync();
    }
} 