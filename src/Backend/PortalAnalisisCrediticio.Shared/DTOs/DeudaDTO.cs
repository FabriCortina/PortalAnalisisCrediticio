namespace PortalAnalisisCrediticio.Shared.DTOs;

public class DeudaDTO
{
    public int Id { get; set; }
    public int InformacionFinancieraId { get; set; }
    public string TipoDeuda { get; set; }
    public string EntidadFinanciera { get; set; }
    public decimal Monto { get; set; }
    public decimal TasaInteres { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string Estado { get; set; }
    public string Moneda { get; set; }
    public string FuenteInformacion { get; set; }
    public string Observaciones { get; set; }

    // Relaciones
    public InformacionFinancieraDTO InformacionFinanciera { get; set; }
} 