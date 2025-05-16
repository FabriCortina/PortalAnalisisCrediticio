using System.ComponentModel.DataAnnotations;
using PortalAnalisisCrediticio.Shared.DTOs.Cliente;
using PortalAnalisisCrediticio.Shared.DTOs.Compania;

namespace PortalAnalisisCrediticio.Shared.DTOs.ClienteCompania;

public class ClienteCompaniaDTO
{
    public int Id { get; set; }
    
    [Required]
    public int ClienteId { get; set; }
    
    [Required]
    public int CompaniaId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Cargo { get; set; }
    
    [Required]
    public DateTime FechaInicio { get; set; }
    
    public DateTime? FechaFin { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    // Relaciones
    public ClienteDTO Cliente { get; set; }
    public CompaniaDTO Compania { get; set; }
} 