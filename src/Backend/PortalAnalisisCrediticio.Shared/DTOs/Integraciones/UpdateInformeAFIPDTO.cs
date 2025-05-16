using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

public class UpdateInformeAFIPDTO
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
    
    // Datos específicos de AFIP
    [Required]
    public bool Monotributista { get; set; }
    
    [Required]
    public bool ResponsableInscripto { get; set; }
    
    public DateTime? FechaInicioActividades { get; set; }
    
    [MaxLength(50)]
    public string CategoriaMonotributo { get; set; }
    
    [MaxLength(50)]
    public string ActividadPrincipal { get; set; }
    
    public decimal? IngresosBrutos { get; set; }
    
    public decimal? ImpuestosPagados { get; set; }
    
    public bool? TieneDeuda { get; set; }
    
    public decimal? MontoDeuda { get; set; }
    
    [MaxLength(200)]
    public string EstadoFiscal { get; set; }
    
    public DateTime? FechaUltimaDeclaracion { get; set; }
    
    [MaxLength(50)]
    public string TipoDeclaracion { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
} 