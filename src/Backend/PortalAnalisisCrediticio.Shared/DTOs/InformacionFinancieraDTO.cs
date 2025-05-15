namespace PortalAnalisisCrediticio.Shared.DTOs;

public class InformacionFinancieraDTO
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
    public ClienteDTO Cliente { get; set; }
    public ICollection<EstadoFinancieroDTO> EstadosFinancieros { get; set; }
    public ICollection<FlujoCajaProyectadoDTO> FlujosCajaProyectados { get; set; }
    public ICollection<DeudaDTO> Deudas { get; set; }
} 