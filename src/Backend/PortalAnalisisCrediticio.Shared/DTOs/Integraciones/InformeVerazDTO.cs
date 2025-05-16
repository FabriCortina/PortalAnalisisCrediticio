using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

public class InformeVerazDTO
{
    public string Estado { get; set; }
    public string Score { get; set; }
    public string Riesgo { get; set; }
    public string UltimaActualizacion { get; set; }
    public string Observaciones { get; set; }
    public List<DeudaVerazDTO> Deudas { get; set; }
    public List<ConsultaVerazDTO> Consultas { get; set; }
    public string SituacionCrediticia { get; set; }
    public decimal MontoTotalDeuda { get; set; }
    public int CantidadDeudas { get; set; }
    public string CategoriaRiesgo { get; set; }
}

public class DeudaVerazDTO
{
    public string Entidad { get; set; }
    public string TipoDeuda { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string Estado { get; set; }
}

public class ConsultaVerazDTO
{
    public string Entidad { get; set; }
    public DateTime FechaConsulta { get; set; }
    public string TipoConsulta { get; set; }
    public string Resultado { get; set; }
} 