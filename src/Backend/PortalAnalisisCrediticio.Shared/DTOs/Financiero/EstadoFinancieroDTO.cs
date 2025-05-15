using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Financiero;

public class EstadoFinancieroDTO
{
    public int Id { get; set; }
    
    [Required]
    public DateTime Fecha { get; set; }
    
    [Required]
    public decimal Activos { get; set; }
    
    [Required]
    public decimal Pasivos { get; set; }
    
    [Required]
    public decimal PatrimonioNeto { get; set; }
} 