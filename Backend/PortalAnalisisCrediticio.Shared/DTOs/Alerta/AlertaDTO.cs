using System;

namespace PortalAnalisisCrediticio.Shared.DTOs.Alerta
{
    public class AlertaDTO
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public string TipoEvento { get; set; }
        public string MedioNotificacion { get; set; }
        public int? DiasAntesVencimiento { get; set; }
        public string? EstadoFinancieroObjetivo { get; set; }
        public bool Activa { get; set; }
        public string? EmailDestino { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaUltimaNotificacion { get; set; }
    }
} 