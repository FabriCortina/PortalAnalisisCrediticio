using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Credito;

public class PagoCreditoDTO
{
    public int Id { get; set; }
    public int CreditoId { get; set; }
    public DateTime FechaPago { get; set; }
    public decimal MontoPagado { get; set; }
    public int NumeroCuota { get; set; }
    public DateTime VencimientoCuota { get; set; }
    public string EstadoCuota { get; set; }
}

public class CreatePagoCreditoDTO
{
    [Required]
    public int CreditoId { get; set; }
    [Required]
    public DateTime FechaPago { get; set; }
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal MontoPagado { get; set; }
    [Required]
    public int NumeroCuota { get; set; }
    [Required]
    public DateTime VencimientoCuota { get; set; }
    [Required]
    [StringLength(30)]
    public string EstadoCuota { get; set; }
}

public class UpdatePagoCreditoDTO
{
    [Required]
    [StringLength(30)]
    public string EstadoCuota { get; set; }
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal MontoPagado { get; set; }
    public DateTime? FechaPago { get; set; }
} 