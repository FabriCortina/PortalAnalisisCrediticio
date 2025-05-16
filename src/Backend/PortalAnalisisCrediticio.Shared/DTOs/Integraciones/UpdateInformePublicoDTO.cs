using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

public class UpdateInformePublicoDTO
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
    
    // Datos específicos de Informe Público
    [Required]
    [MaxLength(100)]
    public string NombreCompleto { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoDocumento { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string NumeroDocumento { get; set; }
    
    public DateTime? FechaNacimiento { get; set; }
    
    [MaxLength(100)]
    public string Nacionalidad { get; set; }
    
    [MaxLength(200)]
    public string Domicilio { get; set; }
    
    [MaxLength(100)]
    public string Ciudad { get; set; }
    
    [MaxLength(100)]
    public string Provincia { get; set; }
    
    [MaxLength(100)]
    public string Pais { get; set; }
    
    [MaxLength(20)]
    public string CodigoPostal { get; set; }
    
    public bool? TieneAntecedentesPenales { get; set; }
    
    public bool? TieneAntecedentesJudiciales { get; set; }
    
    public bool? TieneRestricciones { get; set; }
    
    [MaxLength(500)]
    public string DetalleRestricciones { get; set; }
    
    public DateTime? FechaUltimaActualizacion { get; set; }
    
    [MaxLength(500)]
    public string ObservacionesAdicionales { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
} 