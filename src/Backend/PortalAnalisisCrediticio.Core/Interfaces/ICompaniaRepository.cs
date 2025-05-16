using PortalAnalisisCrediticio.Core.Domain.Entities;

namespace PortalAnalisisCrediticio.Core.Interfaces
{
    public interface ICompaniaRepository
    {
        Task<IEnumerable<Compania>> GetAllAsync();
        Task<Compania> GetByIdAsync(int id);
        Task<Compania> AddAsync(Compania compania);
        Task UpdateAsync(Compania compania);
        Task DeleteAsync(int id);
        Task<Compania> GetByCodigoAsync(string codigo);
        Task<IEnumerable<Compania>> GetByClienteIdAsync(int clienteId);
    }
} 