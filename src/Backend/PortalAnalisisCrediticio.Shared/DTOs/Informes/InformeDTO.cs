using System.ComponentModel.DataAnnotations;
using PortalAnalisisCrediticio.Shared.DTOs.Cliente;
using PortalAnalisisCrediticio.Shared.DTOs.Producto;
using PortalAnalisisCrediticio.Shared.DTOs.AnalisisRiesgo;

namespace PortalAnalisisCrediticio.Shared.DTOs.Informes;

public class InformeDTO
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public DateTime FechaGeneracion { get; set; }
    
    // Datos del cliente
    public ClienteDTO Cliente { get; set; }
    
    // Productos solicitados
    public List<SolicitudProductoDTO> ProductosSolicitados { get; set; }
    
    // Análisis de riesgo
    public InformeRiesgoDTO AnalisisRiesgo { get; set; }
    
    // Recomendaciones
    public string Recomendaciones { get; set; }
    
    // Historial de informes previos
    public List<InformeHistoricoDTO> HistorialInformes { get; set; }
}

public class InformeHistoricoDTO
{
    public int Id { get; set; }
    public DateTime FechaGeneracion { get; set; }
    public string NivelRiesgo { get; set; }
    public bool RecomendacionOtorgarCredito { get; set; }
    public decimal TasaInteresSugerida { get; set; }
    public string GarantiasAdicionalesSugeridas { get; set; }
} 