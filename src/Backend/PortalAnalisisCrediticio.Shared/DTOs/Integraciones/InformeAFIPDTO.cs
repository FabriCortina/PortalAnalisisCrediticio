using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

public class InformeAFIPDTO
{
    public string Estado { get; set; }
    public string Score { get; set; }
    public string Riesgo { get; set; }
    public string UltimaActualizacion { get; set; }
    public string Observaciones { get; set; }
    public List<ImpuestoAFIPDTO> Impuestos { get; set; }
    public List<RetencionAFIPDTO> Retenciones { get; set; }
    public string SituacionFiscal { get; set; }
    public decimal MontoTotalImpuestos { get; set; }
    public int CantidadImpuestos { get; set; }
    public string CategoriaRiesgo { get; set; }
    public List<DeclaracionAFIPDTO> Declaraciones { get; set; }
}

public class ImpuestoAFIPDTO
{
    public string TipoImpuesto { get; set; }
    public string Periodo { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string Estado { get; set; }
}

public class RetencionAFIPDTO
{
    public string TipoRetencion { get; set; }
    public string Periodo { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaRetencion { get; set; }
    public string Estado { get; set; }
}

public class DeclaracionAFIPDTO
{
    public string TipoDeclaracion { get; set; }
    public string Periodo { get; set; }
    public DateTime FechaPresentacion { get; set; }
    public string Estado { get; set; }
    public decimal MontoDeclarado { get; set; }
} 