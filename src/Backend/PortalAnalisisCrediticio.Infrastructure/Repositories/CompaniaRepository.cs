using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;

namespace PortalAnalisisCrediticio.Infrastructure.Repositories
{
    public class CompaniaRepository : ICompaniaRepository
    {
        private readonly ApplicationDbContext _context;

        public CompaniaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Compania>> GetAllAsync()
        {
            return await _context.Companias
                .Include(c => c.ClienteCompanias)
                    .ThenInclude(cc => cc.Cliente)
                .ToListAsync();
        }

        public async Task<Compania> GetByIdAsync(int id)
        {
            return await _context.Companias
                .Include(c => c.ClienteCompanias)
                    .ThenInclude(cc => cc.Cliente)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Compania> AddAsync(Compania compania)
        {
            await _context.Companias.AddAsync(compania);
            await _context.SaveChangesAsync();
            return compania;
        }

        public async Task UpdateAsync(Compania compania)
        {
            _context.Entry(compania).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var compania = await _context.Companias.FindAsync(id);
            if (compania != null)
            {
                _context.Companias.Remove(compania);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Compania> GetByCodigoAsync(string codigo)
        {
            return await _context.Companias
                .Include(c => c.ClienteCompanias)
                    .ThenInclude(cc => cc.Cliente)
                .FirstOrDefaultAsync(c => c.Codigo == codigo);
        }

        public async Task<IEnumerable<Compania>> GetByClienteIdAsync(int clienteId)
        {
            return await _context.Companias
                .Include(c => c.ClienteCompanias)
                    .ThenInclude(cc => cc.Cliente)
                .Where(c => c.ClienteCompanias.Any(cc => cc.ClienteId == clienteId))
                .ToListAsync();
        }
    }
} 