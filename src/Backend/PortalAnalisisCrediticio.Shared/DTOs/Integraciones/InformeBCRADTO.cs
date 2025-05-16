using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

public class InformeBCRADTO
{
    public string Estado { get; set; }
    public string Score { get; set; }
    public string Riesgo { get; set; }
    public string UltimaActualizacion { get; set; }
    public string Observaciones { get; set; }
    public List<DeudaBCRADTO> Deudas { get; set; }
    public List<GarantiaBCRADTO> Garantias { get; set; }
    public string SituacionCrediticia { get; set; }
    public decimal MontoTotalDeuda { get; set; }
    public int CantidadDeudas { get; set; }
    public string CategoriaRiesgo { get; set; }
    public List<OperacionBCRADTO> Operaciones { get; set; }
}

public class DeudaBCRADTO
{
    public string Entidad { get; set; }
    public string TipoDeuda { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string Estado { get; set; }
    public string ClasificacionRiesgo { get; set; }
}

public class GarantiaBCRADTO
{
    public string TipoGarantia { get; set; }
    public decimal ValorGarantia { get; set; }
    public DateTime FechaConstitucion { get; set; }
    public string Estado { get; set; }
    public string EntidadGarante { get; set; }
}

public class OperacionBCRADTO
{
    public string TipoOperacion { get; set; }
    public DateTime FechaOperacion { get; set; }
    public decimal Monto { get; set; }
    public string Estado { get; set; }
    public string Entidad { get; set; }
} 