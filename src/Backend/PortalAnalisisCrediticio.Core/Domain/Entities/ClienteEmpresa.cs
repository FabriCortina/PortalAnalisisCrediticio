using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class ClienteEmpresa
{
    [Required]
    public int ClienteId { get; set; }

    [Required]
    public int EmpresaId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Cargo { get; set; }

    [Required]
    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    // Relaciones
    public Cliente Cliente { get; set; }
    public Empresa Empresa { get; set; }
} 