namespace PortalAnalisisCrediticio.Core.Entities;

public class InformeExterno
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string Fuente { get; set; } // Nosis, Veraz, Infoexperto, BCRA, AFIP, Publico
    public string Estado { get; set; }
    public string Score { get; set; }
    public string Riesgo { get; set; }
    public string UltimaActualizacion { get; set; }
    public string Observaciones { get; set; }
    public DateTime FechaConsulta { get; set; } = DateTime.Now;
    public Cliente Cliente { get; set; }
} 