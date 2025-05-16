using System;

namespace PortalAnalisisCrediticio.Core.Domain.Entities
{
    public class Alerta
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; } // Puede ser null si es global o por tipo de evento
        public string TipoEvento { get; set; } // Ej: "VencimientoCuota", "CambioEstadoFinanciero"
        public string MedioNotificacion { get; set; } // "Email", "Push"
        public int? DiasAntesVencimiento { get; set; } // Solo para alertas de vencimiento
        public string? EstadoFinancieroObjetivo { get; set; } // Solo para alertas de cambio de estado financiero
        public bool Activa { get; set; }
        public string? EmailDestino { get; set; } // Si se quiere notificar a un email específico
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaUltimaNotificacion { get; set; }
    }
} 