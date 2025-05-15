namespace PortalAnalisisCrediticio.Core.Entities;

public class InformeRiesgo
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string NivelRiesgo { get; set; }
    public string Justificacion { get; set; }
    public bool RecomendacionOtorgarCredito { get; set; }
    public decimal TasaInteresSugerida { get; set; }
    public string GarantiasAdicionalesSugeridas { get; set; }
    public int PlazoMaximoSugerido { get; set; }
    public DateTime FechaAnalisis { get; set; } = DateTime.Now;
    
    // Factores de análisis
    public double ScoreHistorialPagos { get; set; }
    public double ScoreSituacionFinanciera { get; set; }
    public double ScoreInformesExternos { get; set; }
    public double ScoreGarantias { get; set; }
    
    // Relaciones
    public Cliente Cliente { get; set; }
} 