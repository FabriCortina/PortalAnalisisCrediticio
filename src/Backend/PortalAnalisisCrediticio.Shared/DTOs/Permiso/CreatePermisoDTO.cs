using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Permiso;

public class CreatePermisoDTO
{
    [Required]
    [MaxLength(50)]
    public string Codigo { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Descripcion { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Modulo { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; }
    
    [Required]
    public bool Activo { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UsuarioModificacion { get; set; }
    
    [Required]
    public int NivelAcceso { get; set; }
    
    [Required]
    public bool RequiereAprobacion { get; set; }
    
    [Required]
    public bool EsSensible { get; set; }
    
    [Required]
    public bool RegistraAuditoria { get; set; }
    
    [Required]
    public bool EsObligatorio { get; set; }
    
    [Required]
    public int Orden { get; set; }
} 