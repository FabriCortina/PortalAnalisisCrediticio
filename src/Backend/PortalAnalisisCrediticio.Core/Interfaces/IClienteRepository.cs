using PortalAnalisisCrediticio.Core.Domain.Entities;

namespace PortalAnalisisCrediticio.Core.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente> GetByIdAsync(int id);
        Task<Cliente> AddAsync(Cliente cliente);
        Task UpdateAsync(Cliente cliente);
        Task DeleteAsync(int id);
        Task<Cliente> GetByCUIT_CUILAsync(string cuit_cuil);
        Task<IEnumerable<Cliente>> GetByCompaniaIdAsync(int companiaId);
    }
} 