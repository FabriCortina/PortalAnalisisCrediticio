using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Entities;

namespace PortalAnalisisCrediticio.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<InformacionFinanciera> InformacionFinanciera { get; set; }
        public DbSet<EstadoFinanciero> EstadosFinancieros { get; set; }
        public DbSet<FlujoCajaProyectado> FlujosCajaProyectados { get; set; }
        public DbSet<Deuda> Deudas { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<ClienteEmpresa> ClienteEmpresas { get; set; }
        public DbSet<InformeExterno> InformesExternos { get; set; }
        public DbSet<InformeRiesgo> InformesRiesgo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CUIT).IsRequired().HasMaxLength(11);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Telefono).HasMaxLength(20);
            });

            // Configuración de Información Financiera
            modelBuilder.Entity<InformacionFinanciera>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Cliente)
                    .WithOne(c => c.InformacionFinanciera)
                    .HasForeignKey<InformacionFinanciera>(e => e.ClienteId);
            });

            // Configuración de Estado Financiero
            modelBuilder.Entity<EstadoFinanciero>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.InformacionFinanciera)
                    .WithMany(i => i.EstadosFinancieros)
                    .HasForeignKey(e => e.InformacionFinancieraId);
            });

            // Configuración de Flujo de Caja Proyectado
            modelBuilder.Entity<FlujoCajaProyectado>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.InformacionFinanciera)
                    .WithMany(i => i.FlujosCajaProyectados)
                    .HasForeignKey(e => e.InformacionFinancieraId);
            });

            // Configuración de Deuda
            modelBuilder.Entity<Deuda>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.InformacionFinanciera)
                    .WithMany(i => i.Deudas)
                    .HasForeignKey(e => e.InformacionFinancieraId);
            });

            // Configuración de Empresa
            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CUIT).IsRequired().HasMaxLength(11);
            });

            // Configuración de ClienteEmpresa
            modelBuilder.Entity<ClienteEmpresa>(entity =>
            {
                entity.HasKey(e => new { e.ClienteId, e.EmpresaId });
                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.ClienteEmpresas)
                    .HasForeignKey(e => e.ClienteId);
                entity.HasOne(e => e.Empresa)
                    .WithMany(e => e.ClienteEmpresas)
                    .HasForeignKey(e => e.EmpresaId);
            });

            // Configuración de InformeRiesgo
            modelBuilder.Entity<InformeRiesgo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Cliente)
                    .WithMany()
                    .HasForeignKey(e => e.ClienteId);
                entity.Property(e => e.NivelRiesgo).IsRequired();
                entity.Property(e => e.Justificacion).IsRequired();
                entity.Property(e => e.TasaInteresSugerida).HasPrecision(5, 2);
            });
        }
    }
} 