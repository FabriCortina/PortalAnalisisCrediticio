using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

public class CreateInformeBCRADTO
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
    
    // Datos específicos de BCRA
    [Required]
    public bool TieneCuentasBancarias { get; set; }
    
    public int? CantidadCuentas { get; set; }
    
    public decimal? TotalDeudaBancaria { get; set; }
    
    public decimal? LineaCreditoDisponible { get; set; }
    
    public decimal? LineaCreditoUtilizada { get; set; }
    
    [MaxLength(50)]
    public string CalificacionBCRA { get; set; }
    
    public bool? TieneChequesRechazados { get; set; }
    
    public int? CantidadChequesRechazados { get; set; }
    
    public decimal? MontoChequesRechazados { get; set; }
    
    public bool? TieneProtestos { get; set; }
    
    public int? CantidadProtestos { get; set; }
    
    public decimal? MontoProtestos { get; set; }
    
    [MaxLength(200)]
    public string EstadoBancario { get; set; }
    
    public DateTime? FechaUltimaActualizacion { get; set; }
} 