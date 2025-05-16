using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Entities;

namespace PortalAnalisisCrediticio.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Alerta> Alertas { get; set; }
    }
} 