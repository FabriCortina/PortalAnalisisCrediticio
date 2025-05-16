using System;
using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Informes;

public class InformeRiesgoDTO
{
    public int Id { get; set; }
    
    [Required]
    public int ClienteId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string NivelRiesgo { get; set; }
    
    [Required]
    public decimal ScoreTotal { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string Justificacion { get; set; }
    
    [Required]
    public bool RecomendacionOtorgarCredito { get; set; }
    
    [Required]
    public decimal TasaInteresSugerida { get; set; }
    
    [MaxLength(500)]
    public string GarantiasAdicionalesSugeridas { get; set; }
    
    [Required]
    public int PlazoMaximoSugerido { get; set; }
    
    [Required]
    public DateTime FechaAnalisis { get; set; }
    
    [Required]
    public double ScoreHistorialPagos { get; set; }
    
    [Required]
    public double ScoreSituacionFinanciera { get; set; }
    
    [Required]
    public double ScoreInformesExternos { get; set; }
    
    [Required]
    public double ScoreGarantias { get; set; }
    
    // Propiedades de navegación
    public string NombreCliente { get; set; }
    public string CodigoCliente { get; set; }
} 