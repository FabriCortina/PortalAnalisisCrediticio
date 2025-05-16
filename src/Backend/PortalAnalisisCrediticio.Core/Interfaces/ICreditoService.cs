using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Shared.DTOs.Credito;

namespace PortalAnalisisCrediticio.Core.Interfaces
{
    public interface ICreditoService
    {
        // Créditos
        Task<CreditoDTO> GetByIdAsync(int id);
        Task<IEnumerable<CreditoDTO>> GetByClienteIdAsync(int clienteId);
        Task<CreditoDTO> CreateAsync(CreateCreditoDTO creditoDto);
        Task<CreditoDTO> UpdateAsync(int id, UpdateCreditoDTO creditoDto);
        Task DeleteAsync(int id);

        // Pagos
        Task<PagoCreditoDTO> RegistrarPagoAsync(CreatePagoCreditoDTO pagoDto);
        Task<IEnumerable<PagoCreditoDTO>> GetPagosByCreditoIdAsync(int creditoId);
        Task<IEnumerable<CreditoDTO>> GetHistorialCreditosPorClienteAsync(int clienteId);
    }
} 