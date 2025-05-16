using System;
using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Alerta
{
    public class UpdateAlertaDTO
    {
        [Required]
        public string MedioNotificacion { get; set; }
        public int? DiasAntesVencimiento { get; set; }
        public string? EstadoFinancieroObjetivo { get; set; }
        public bool Activa { get; set; }
        public string? EmailDestino { get; set; }
    }
} 