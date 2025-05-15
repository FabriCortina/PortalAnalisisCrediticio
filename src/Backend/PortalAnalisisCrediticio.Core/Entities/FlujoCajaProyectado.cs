namespace PortalAnalisisCrediticio.Core.Entities;

public class FlujoCajaProyectado
{
    public int Id { get; set; }
    public int InformacionFinancieraId { get; set; }
    public DateTime Fecha { get; set; }
    public string Periodo { get; set; }
    public decimal EntradasEfectivo { get; set; }
    public decimal SalidasEfectivo { get; set; }
    public decimal SaldoInicial { get; set; }
    public decimal SaldoFinal { get; set; }
    public string Moneda { get; set; }
    public string FuenteInformacion { get; set; }
    public string Observaciones { get; set; }

    // Relaciones
    public InformacionFinanciera InformacionFinanciera { get; set; }
} 