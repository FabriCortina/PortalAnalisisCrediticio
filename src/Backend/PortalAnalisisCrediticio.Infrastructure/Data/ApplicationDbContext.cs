using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Domain.Entities;

namespace PortalAnalisisCrediticio.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AnalisisCrediticio> AnalisisCrediticios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Alerta> Alertas { get; set; }
        public DbSet<Compania> Companias { get; set; }
        public DbSet<ClienteCompania> ClienteCompanias { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<ClienteEmpresa> ClienteEmpresas { get; set; }
        public DbSet<InformacionFinanciera> InformacionFinanciera { get; set; }
        public DbSet<EstadoFinanciero> EstadosFinancieros { get; set; }
        public DbSet<FlujoCajaProyectado> FlujosCajaProyectados { get; set; }
        public DbSet<Deuda> Deudas { get; set; }
        public DbSet<Credito> Creditos { get; set; }
        public DbSet<SolicitudProducto> SolicitudesProducto { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<InformeExterno> InformesExternos { get; set; }
        public DbSet<InformeRiesgo> InformesRiesgo { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<AuditoriaAcceso> AuditoriaAccesos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductoSolicitud> ProductoSolicitudes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de AnalisisCrediticio
            modelBuilder.Entity<AnalisisCrediticio>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MontoSolicitado).HasPrecision(18, 2);
                entity.Property(e => e.NivelRiesgo).IsRequired();
                entity.Property(e => e.Estado).IsRequired();

                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.AnalisisCrediticios)
                    .HasForeignKey(e => e.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.TipoDocumento).IsRequired();
                entity.Property(e => e.NumeroDocumento).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Telefono).HasMaxLength(20);
            });

            // Configuración de Alerta
            modelBuilder.Entity<Alerta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Tipo).IsRequired();
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Nivel).IsRequired();

                entity.HasOne(e => e.AnalisisCrediticio)
                    .WithMany(a => a.Alertas)
                    .HasForeignKey(e => e.AnalisisCrediticioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Compania
            modelBuilder.Entity<Compania>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(20);
                entity.Property(e => e.MonedaPrincipal).IsRequired().HasMaxLength(3);
                entity.Property(e => e.Pais).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ConfiguracionRegional).IsRequired().HasMaxLength(50);
            });

            // Configuración de ClienteCompania
            modelBuilder.Entity<ClienteCompania>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.ClienteCompanias)
                    .HasForeignKey(e => e.ClienteId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Compania)
                    .WithMany(c => c.ClienteCompanias)
                    .HasForeignKey(e => e.CompaniaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Empresa
            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Ruc).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Direccion).HasMaxLength(200);
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

            // Configuración de InformacionFinanciera
            modelBuilder.Entity<InformacionFinanciera>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Cliente)
                    .WithOne(c => c.InformacionFinanciera)
                    .HasForeignKey<InformacionFinanciera>(e => e.ClienteId);
            });

            // Configuración de Credito
            modelBuilder.Entity<Credito>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Monto).HasPrecision(18, 2);
                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.Creditos)
                    .HasForeignKey(e => e.ClienteId);
            });

            // Configuración de SolicitudProducto
            modelBuilder.Entity<SolicitudProducto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Monto).HasPrecision(18, 2);
                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.SolicitudesProducto)
                    .HasForeignKey(e => e.ClienteId);
            });

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            });

            // Configuración de Actividad
            modelBuilder.Entity<Actividad>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Usuario).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Accion).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Entidad).IsRequired().HasMaxLength(50);
            });

            // Configuración de Notificacion
            modelBuilder.Entity<Notificacion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Mensaje).IsRequired();
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(50);
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

            // Configuración de InformeRiesgo
            modelBuilder.Entity<InformeRiesgo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NivelRiesgo).IsRequired();
                entity.Property(e => e.ScoreTotal).HasPrecision(5, 2);
                entity.Property(e => e.TasaInteresSugerida).HasPrecision(5, 2);
                entity.Property(e => e.FechaAnalisis).IsRequired();
                
                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.InformesRiesgo)
                    .HasForeignKey(e => e.ClienteId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de InformeExterno
            modelBuilder.Entity<InformeExterno>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Fuente).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FechaInforme).IsRequired();
                entity.Property(e => e.TipoInforme).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Contenido).IsRequired();
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(50);

                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.InformesExternos)
                    .HasForeignKey(e => e.ClienteId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Permiso
            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Recurso).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Accion).IsRequired().HasMaxLength(50);
                
                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.Permisos)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de AuditoriaAcceso
            modelBuilder.Entity<AuditoriaAcceso>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TipoAccion).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasMaxLength(200);
                entity.Property(e => e.IP).HasMaxLength(50);
                entity.Property(e => e.UserAgent).HasMaxLength(200);
                
                entity.HasOne(e => e.Usuario)
                    .WithMany(u => u.AuditoriaAccesos)
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CodigoInterno).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Moneda).IsRequired().HasMaxLength(3);
                entity.HasIndex(e => e.CodigoInterno).IsUnique();
            });

            // Configuración de ProductoSolicitud
            modelBuilder.Entity<ProductoSolicitud>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Cantidad).IsRequired();
                entity.Property(e => e.Subtotal).HasColumnType("decimal(18,2)");
                
                entity.HasOne(e => e.Producto)
                    .WithMany(p => p.ProductoSolicitudes)
                    .HasForeignKey(e => e.ProductoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SolicitudProducto)
                    .WithMany(s => s.ProductoSolicitudes)
                    .HasForeignKey(e => e.SolicitudProductoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de SolicitudProducto
            modelBuilder.Entity<SolicitudProducto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FechaSolicitud).IsRequired();
                entity.Property(e => e.MontoTotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PagoInicial).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PorcentajeFinanciacion).HasColumnType("decimal(5,2)");
                entity.Property(e => e.MontoFinanciado).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MontoCuota).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Observaciones).HasMaxLength(500);

                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.SolicitudesProducto)
                    .HasForeignKey(e => e.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
} 