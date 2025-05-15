namespace PortalAnalisisCrediticio.Core.Entities;

public class Deuda
{
    public int Id { get; set; }
    public int InformacionFinancieraId { get; set; }
    public string TipoDeuda { get; set; }
    public string EntidadFinanciera { get; set; }
    public decimal MontoOriginal { get; set; }
    public decimal MontoActual { get; set; }
    public decimal TasaInteres { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string Estado { get; set; }
    public string Moneda { get; set; }
    public string FuenteInformacion { get; set; }
    public string Observaciones { get; set; }

    // Relaciones
    public InformacionFinanciera InformacionFinanciera { get; set; }
} 