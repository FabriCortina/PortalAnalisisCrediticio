using System;
using System.ComponentModel.DataAnnotations;
using PortalAnalisisCrediticio.Shared.DTOs.Cliente;

namespace PortalAnalisisCrediticio.Shared.DTOs.AnalisisRiesgo;

public class InformeRiesgoDTO
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    
    [Required]
    public string NivelRiesgo { get; set; } // Alto, Medio, Bajo
    
    [Required]
    public string Justificacion { get; set; }
    
    [Required]
    public bool RecomendacionOtorgarCredito { get; set; }
    
    [Required]
    public decimal TasaInteresSugerida { get; set; }
    
    [Required]
    public string GarantiasAdicionalesSugeridas { get; set; }
    
    [Required]
    public int PlazoMaximoSugerido { get; set; } // en meses
    
    public DateTime FechaAnalisis { get; set; } = DateTime.Now;
    
    // Factores de análisis
    public double ScoreHistorialPagos { get; set; }
    public double ScoreSituacionFinanciera { get; set; }
    public double ScoreInformesExternos { get; set; }
    public double ScoreGarantias { get; set; }
    
    // Relaciones
    public ClienteDTO Cliente { get; set; }
} 