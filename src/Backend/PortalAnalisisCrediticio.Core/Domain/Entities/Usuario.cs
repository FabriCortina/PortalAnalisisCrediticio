using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }

    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public string PasswordHash { get; set; }

    [Required]
    [MaxLength(50)]
    public string Rol { get; set; } // Administrador, AnalistaCredito, Vendedor

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public DateTime? UltimoAcceso { get; set; }

    // Campos para cumplir con Ley 25.326
    public bool AceptoTerminos { get; set; }
    public DateTime? FechaAceptacionTerminos { get; set; }
    public bool AceptoMarketing { get; set; }
    public DateTime? FechaAceptacionMarketing { get; set; }

    // Relaciones
    public ICollection<Permiso> Permisos { get; set; }
    public ICollection<AuditoriaAcceso> AuditoriaAccesos { get; set; }
} 