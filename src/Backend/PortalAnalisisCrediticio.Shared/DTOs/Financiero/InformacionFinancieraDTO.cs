using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Financiero;

public class InformacionFinancieraDTO
{
    public int Id { get; set; }
    
    [Required]
    public decimal IngresosMensuales { get; set; }
    
    [Required]
    public decimal GastosMensuales { get; set; }
    
    [Required]
    public decimal PatrimonioNeto { get; set; }
    
    // Relaciones
    public List<EstadoFinancieroDTO> EstadosFinancieros { get; set; }
    public List<FlujoCajaProyectadoDTO> FlujosCajaProyectados { get; set; }
    public List<DeudaDTO> Deudas { get; set; }
} 