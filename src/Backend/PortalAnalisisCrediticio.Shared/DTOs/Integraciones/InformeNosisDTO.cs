using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

public class InformeNosisDTO
{
    public string Estado { get; set; }
    public string Score { get; set; }
    public string Riesgo { get; set; }
    public string UltimaActualizacion { get; set; }
    public string Observaciones { get; set; }
    public List<DeudaNosisDTO> Deudas { get; set; }
    public List<ProtestoNosisDTO> Protestos { get; set; }
    public string SituacionCrediticia { get; set; }
    public decimal MontoTotalDeuda { get; set; }
    public int CantidadDeudas { get; set; }
    public string CategoriaRiesgo { get; set; }
    public List<ChequeNosisDTO> Cheques { get; set; }
}

public class DeudaNosisDTO
{
    public string Entidad { get; set; }
    public string TipoDeuda { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string Estado { get; set; }
}

public class ProtestoNosisDTO
{
    public string Entidad { get; set; }
    public DateTime FechaProtesto { get; set; }
    public string TipoProtesto { get; set; }
    public decimal Monto { get; set; }
    public string Estado { get; set; }
}

public class ChequeNosisDTO
{
    public string Banco { get; set; }
    public string NumeroCheque { get; set; }
    public DateTime FechaEmision { get; set; }
    public decimal Monto { get; set; }
    public string Estado { get; set; }
} 