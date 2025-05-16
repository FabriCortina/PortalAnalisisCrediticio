using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class InformacionFinanciera
{
    public int Id { get; set; }

    [Required]
    public int ClienteId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal IngresosMensuales { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal GastosMensuales { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PatrimonioNeto { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal CapacidadPago { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal RatioEndeudamiento { get; set; }

    [Required]
    // Relaciones
    public Cliente Cliente { get; set; }
    public ICollection<EstadoFinanciero> EstadosFinancieros { get; set; }
    public ICollection<FlujoCajaProyectado> FlujosCajaProyectados { get; set; }
    public ICollection<Deuda> Deudas { get; set; }
} 