using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class AuditoriaAcceso
{
    public int Id { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    [Required]
    public DateTime FechaAcceso { get; set; } = DateTime.Now;

    [Required]
    [MaxLength(50)]
    public string TipoAccion { get; set; } // Login, Logout, AccesoDenegado

    [MaxLength(200)]
    public string Descripcion { get; set; }

    [MaxLength(50)]
    public string IP { get; set; }

    [MaxLength(200)]
    public string UserAgent { get; set; }

    // Relaciones
    public Usuario Usuario { get; set; }
} 