using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class Permiso
{
    public int Id { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Recurso { get; set; }

    [Required]
    [MaxLength(50)]
    public string Accion { get; set; }

    [Required]
    public bool Permitido { get; set; }

    // Relaciones
    public Usuario Usuario { get; set; }
} 