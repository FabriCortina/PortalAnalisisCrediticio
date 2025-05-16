using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;

namespace PortalAnalisisCrediticio.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .Include(c => c.ClienteCompanias)
                    .ThenInclude(cc => cc.Compania)
                .ToListAsync();
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.ClienteCompanias)
                    .ThenInclude(cc => cc.Compania)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente> AddAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Cliente> GetByCUIT_CUILAsync(string cuit_cuil)
        {
            return await _context.Clientes
                .Include(c => c.ClienteCompanias)
                    .ThenInclude(cc => cc.Compania)
                .FirstOrDefaultAsync(c => c.CUIT_CUIL == cuit_cuil);
        }

        public async Task<IEnumerable<Cliente>> GetByCompaniaIdAsync(int companiaId)
        {
            return await _context.Clientes
                .Include(c => c.ClienteCompanias)
                    .ThenInclude(cc => cc.Compania)
                .Where(c => c.ClienteCompanias.Any(cc => cc.CompaniaId == companiaId))
                .ToListAsync();
        }
    }
} 