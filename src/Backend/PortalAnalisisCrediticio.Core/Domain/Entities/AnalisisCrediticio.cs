using System;
using System.Collections.Generic;

namespace PortalAnalisisCrediticio.Core.Domain.Entities
{
    public class AnalisisCrediticio
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime FechaAnalisis { get; set; }
        public decimal MontoSolicitado { get; set; }
        public string NivelRiesgo { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        // Relaciones
        public Cliente Cliente { get; set; }
        public ICollection<Alerta> Alertas { get; set; }
    }
} 