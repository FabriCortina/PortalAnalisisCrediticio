using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.AnalisisRiesgo;

public class AnalisisRiesgoRequestDTO
{
    [Required]
    public int ClienteId { get; set; }

    [Required]
    public decimal MontoSolicitado { get; set; }

    [Required]
    public int PlazoMeses { get; set; }

    public List<GarantiaDTO> Garantias { get; set; }

    public InformacionFinancieraDTO InformacionFinanciera { get; set; }

    public List<DeudaDTO> DeudasActuales { get; set; }
}

public class AnalisisRiesgoResponseDTO
{
    public string NivelRiesgo { get; set; } // Alto, Medio, Bajo
    public List<string> Razones { get; set; }
    public bool Recomendacion { get; set; }
    public CondicionesSugeridasDTO CondicionesSugeridas { get; set; }
    public DateTime FechaAnalisis { get; set; }
}

public class CondicionesSugeridasDTO
{
    public decimal TasaInteresSugerida { get; set; }
    public List<string> GarantiasAdicionales { get; set; }
    public decimal MontoMaximoSugerido { get; set; }
    public int PlazoMaximoSugerido { get; set; }
    public List<string> CondicionesEspeciales { get; set; }
} 