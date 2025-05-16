using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

public class CreateInformeInfoexpertoDTO
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
    
    // Datos específicos de Infoexperto
    [Required]
    public decimal ScoreCrediticio { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string NivelRiesgo { get; set; }
    
    public int? CantidadCreditos { get; set; }
    
    public decimal? TotalDeuda { get; set; }
    
    public decimal? CapacidadPago { get; set; }
    
    public decimal? IngresosDeclarados { get; set; }
    
    public decimal? PatrimonioDeclarado { get; set; }
    
    public bool? TieneAntecedentes { get; set; }
    
    public int? CantidadAntecedentes { get; set; }
    
    [MaxLength(200)]
    public string EstadoCrediticio { get; set; }
    
    public DateTime? FechaUltimaActualizacion { get; set; }
    
    [MaxLength(500)]
    public string Recomendaciones { get; set; }
    
    public decimal? LimiteCreditoSugerido { get; set; }
} 