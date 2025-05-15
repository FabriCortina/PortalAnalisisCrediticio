namespace PortalAnalisisCrediticio.Core.Entities;

public class InformacionFinanciera
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public decimal IngresosMensuales { get; set; }
    public decimal GastosMensuales { get; set; }
    public decimal PatrimonioNeto { get; set; }
    public decimal Activos { get; set; }
    public decimal Pasivos { get; set; }
    public string Moneda { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public string FuenteInformacion { get; set; }
    public string Observaciones { get; set; }

    // Relaciones
    public Cliente Cliente { get; set; }
    public ICollection<EstadoFinanciero> EstadosFinancieros { get; set; }
    public ICollection<FlujoCajaProyectado> FlujosCajaProyectados { get; set; }
    public ICollection<Deuda> Deudas { get; set; }
} 