using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Empresa;

public class ClienteEmpresaDTO
{
    [Required]
    public int ClienteId { get; set; }
    
    [Required]
    public int EmpresaId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Cargo { get; set; }
    
    [Required]
    public DateTime FechaInicio { get; set; }
    
    public DateTime? FechaFin { get; set; }
    
    // Relaciones
    public EmpresaDTO Empresa { get; set; }
} 