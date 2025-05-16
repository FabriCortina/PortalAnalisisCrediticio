using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

public class UpdateInformeNosisDTO
{
    [Required]
    [MaxLength(20)]
    public string CUIT { get; set; }
    
    [Required]
    public DateTime FechaConsulta { get; set; }
    
    [Required]
    public bool Activo { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UsuarioModificacion { get; set; }
    
    // Datos específicos de Nosis
    [Required]
    public decimal ScoreNosis { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string CategoriaRiesgo { get; set; }
    
    public int? CantidadProtestos { get; set; }
    
    public decimal? MontoProtestos { get; set; }
    
    public int? CantidadChequesRechazados { get; set; }
    
    public decimal? MontoChequesRechazados { get; set; }
    
    public int? CantidadDeudas { get; set; }
    
    public decimal? MontoDeudas { get; set; }
    
    public bool? TieneAntecedentesJudiciales { get; set; }
    
    public int? CantidadAntecedentesJudiciales { get; set; }
    
    [MaxLength(200)]
    public string EstadoCrediticio { get; set; }
    
    public DateTime? FechaUltimaActualizacion { get; set; }
    
    [MaxLength(500)]
    public string Recomendaciones { get; set; }
    
    public decimal? LimiteCreditoSugerido { get; set; }
    
    [MaxLength(50)]
    public string TipoPersona { get; set; }
    
    public bool? EsEmpresa { get; set; }
    
    public bool? EsPersonaFisica { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
} 