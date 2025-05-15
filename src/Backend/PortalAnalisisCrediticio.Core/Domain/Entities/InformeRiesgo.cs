using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class InformeRiesgo
{
    public int Id { get; set; }
    
    [Required]
    public int ClienteId { get; set; }
    
    [Required]
    public string NivelRiesgo { get; set; }
    
    [Required]
    public decimal ScoreTotal { get; set; }
    
    [Required]
    public string Justificacion { get; set; }
    
    [Required]
    public bool RecomendacionOtorgarCredito { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal TasaInteresSugerida { get; set; }
    
    public string GarantiasAdicionalesSugeridas { get; set; }
    
    [Required]
    public int PlazoMaximoSugerido { get; set; }
    
    [Required]
    public DateTime FechaAnalisis { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public double ScoreHistorialPagos { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public double ScoreSituacionFinanciera { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public double ScoreInformesExternos { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public double ScoreGarantias { get; set; }
    
    // Relaciones
    public Cliente Cliente { get; set; }
} 