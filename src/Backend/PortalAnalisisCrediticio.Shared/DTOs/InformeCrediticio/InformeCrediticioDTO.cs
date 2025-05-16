using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.InformeCrediticio;

public class InformeCrediticioDTO
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int? SolicitudProductoId { get; set; }
    public string NivelRiesgo { get; set; }
    public string Justificacion { get; set; }
    public bool RecomendacionOtorgarCredito { get; set; }
    public decimal TasaInteresSugerida { get; set; }
    public string GarantiasAdicionalesSugeridas { get; set; }
    public int PlazoMaximoSugerido { get; set; }
    public double ScoreHistorialPagos { get; set; }
    public double ScoreSituacionFinanciera { get; set; }
    public double ScoreInformesExternos { get; set; }
    public double ScoreGarantias { get; set; }
    public double ScoreTotal { get; set; }
    public DateTime FechaAnalisis { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public string Observaciones { get; set; }
    public string Autor { get; set; }
    public bool Activo { get; set; }

    // Datos adicionales para la vista
    public ClienteDTO Cliente { get; set; }
    public SolicitudProductoDTO SolicitudProducto { get; set; }
    public List<InformeCrediticioDTO> InformesAnteriores { get; set; }
}

public class CreateInformeCrediticioDTO
{
    [Required]
    public int ClienteId { get; set; }

    public int? SolicitudProductoId { get; set; }

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

    [StringLength(500)]
    public string Observaciones { get; set; }

    [Required]
    [StringLength(100)]
    public string Autor { get; set; }
}

public class UpdateInformeCrediticioDTO
{
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

    [StringLength(500)]
    public string Observaciones { get; set; }
} 