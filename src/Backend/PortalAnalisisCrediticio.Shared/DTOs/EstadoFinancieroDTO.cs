namespace PortalAnalisisCrediticio.Shared.DTOs;

public class EstadoFinancieroDTO
{
    public int Id { get; set; }
    public int InformacionFinancieraId { get; set; }
    public DateTime Fecha { get; set; }
    public string Tipo { get; set; }
    public string Periodo { get; set; }
    public decimal Ventas { get; set; }
    public decimal CostoVentas { get; set; }
    public decimal UtilidadBruta { get; set; }
    public decimal GastosOperativos { get; set; }
    public decimal UtilidadOperativa { get; set; }
    public decimal OtrosIngresos { get; set; }
    public decimal OtrosGastos { get; set; }
    public decimal UtilidadNeta { get; set; }
    public decimal ActivosCorrientes { get; set; }
    public decimal ActivosNoCorrientes { get; set; }
    public decimal PasivosCorrientes { get; set; }
    public decimal PasivosNoCorrientes { get; set; }
    public decimal Capital { get; set; }
    public string Moneda { get; set; }
    public string FuenteInformacion { get; set; }
    public string Observaciones { get; set; }

    // Relaciones
    public InformacionFinancieraDTO InformacionFinanciera { get; set; }
} 