using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class InformeCrediticio
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ClienteId { get; set; }

    [ForeignKey("ClienteId")]
    public Cliente Cliente { get; set; }

    public int? SolicitudProductoId { get; set; }

    [ForeignKey("SolicitudProductoId")]
    public SolicitudProducto SolicitudProducto { get; set; }

    [Required]
    [StringLength(50)]
    public string NivelRiesgo { get; set; }

    [Required]
    public string Justificacion { get; set; }

    [Required]
    public bool RecomendacionOtorgarCredito { get; set; }

    [Required]
    public decimal TasaInteresSugerida { get; set; }

    [Required]
    public string GarantiasAdicionalesSugeridas { get; set; }

    [Required]
    public int PlazoMaximoSugerido { get; set; }

    [Required]
    public double ScoreHistorialPagos { get; set; }

    [Required]
    public double ScoreSituacionFinanciera { get; set; }

    [Required]
    public double ScoreInformesExternos { get; set; }

    [Required]
    public double ScoreGarantias { get; set; }

    [Required]
    public double ScoreTotal { get; set; }

    [Required]
    public DateTime FechaAnalisis { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    [StringLength(500)]
    public string Observaciones { get; set; }

    [Required]
    [StringLength(100)]
    public string Autor { get; set; }

    [Required]
    public bool Activo { get; set; } = true;
} 