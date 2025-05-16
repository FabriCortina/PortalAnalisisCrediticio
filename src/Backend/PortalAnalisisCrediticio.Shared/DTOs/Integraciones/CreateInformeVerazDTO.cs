using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

public class CreateInformeVerazDTO
{
    [Required]
    public int ClienteId { get; set; }
    
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
    
    // Datos específicos de Veraz
    [Required]
    public decimal ScoreVeraz { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string CategoriaRiesgo { get; set; }
    
    public int? CantidadCreditos { get; set; }
    
    public decimal? TotalDeuda { get; set; }
    
    public int? CantidadCuentas { get; set; }
    
    public decimal? LineaCreditoDisponible { get; set; }
    
    public decimal? LineaCreditoUtilizada { get; set; }
    
    public bool? TieneChequesRechazados { get; set; }
    
    public int? CantidadChequesRechazados { get; set; }
    
    public decimal? MontoChequesRechazados { get; set; }
    
    public bool? TieneProtestos { get; set; }
    
    public int? CantidadProtestos { get; set; }
    
    public decimal? MontoProtestos { get; set; }
    
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
} 