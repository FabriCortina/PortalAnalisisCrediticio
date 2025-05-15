using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Financiero;

public class FlujoCajaProyectadoDTO
{
    public int Id { get; set; }
    
    [Required]
    public DateTime Fecha { get; set; }
    
    [Required]
    public decimal Ingresos { get; set; }
    
    [Required]
    public decimal Egresos { get; set; }
    
    [Required]
    public decimal Saldo { get; set; }
} 