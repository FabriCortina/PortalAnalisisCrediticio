namespace PortalAnalisisCrediticio.Core.Entities;

public class Garantia
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string Tipo { get; set; } // Real, Personal, Prendaria, Fiduciaria
    public string Estado { get; set; } // Registrada, Pendiente, Rechazada
    public decimal ValorEstimado { get; set; }
    public decimal MontoGarantizado { get; set; }
    public string Descripcion { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    
    // Relaciones
    public Cliente Cliente { get; set; }
    public Cliente Avalista { get; set; }
    public int? AvalistaId { get; set; }
} 