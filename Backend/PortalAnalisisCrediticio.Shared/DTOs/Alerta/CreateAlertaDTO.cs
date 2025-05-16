using System;
using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Alerta
{
    public class CreateAlertaDTO
    {
        public int? ClienteId { get; set; }
        [Required]
        public string TipoEvento { get; set; }
        [Required]
        public string MedioNotificacion { get; set; }
        public int? DiasAntesVencimiento { get; set; }
        public string? EstadoFinancieroObjetivo { get; set; }
        public bool Activa { get; set; } = true;
        public string? EmailDestino { get; set; }
    }
} 