using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Empresa;

public class EmpresaDTO
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Codigo { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoDocumento { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string NumeroDocumento { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Direccion { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Ciudad { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Provincia { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Pais { get; set; }
    
    [MaxLength(20)]
    public string CodigoPostal { get; set; }
    
    [MaxLength(20)]
    public string Telefono { get; set; }
    
    [MaxLength(20)]
    public string TelefonoAlternativo { get; set; }
    
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }
    
    [MaxLength(100)]
    public string SitioWeb { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoEmpresa { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Sector { get; set; }
    
    [Required]
    public bool Activa { get; set; }
    
    [Required]
    public DateTime FechaRegistro { get; set; }
    
    public DateTime? FechaActualizacion { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [MaxLength(50)]
    public string UsuarioModificacion { get; set; }
    
    [MaxLength(50)]
    public string RepresentanteLegal { get; set; }
    
    [MaxLength(50)]
    public string TipoDocumentoRepresentante { get; set; }
    
    [MaxLength(20)]
    public string NumeroDocumentoRepresentante { get; set; }
    
    [MaxLength(100)]
    public string CargoRepresentante { get; set; }
} 