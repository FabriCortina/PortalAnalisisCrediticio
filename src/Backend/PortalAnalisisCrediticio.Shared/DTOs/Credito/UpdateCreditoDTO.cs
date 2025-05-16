using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Credito
{
    public class UpdateCreditoDTO
    {
        [Required(ErrorMessage = "El tipo de crédito es requerido")]
        [StringLength(50, ErrorMessage = "El tipo de crédito no puede exceder los 50 caracteres")]
        public required string TipoCredito { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La tasa de interés es requerida")]
        public decimal TasaInteres { get; set; }

        [Required(ErrorMessage = "El plazo es requerido")]
        public int Plazo { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [StringLength(50, ErrorMessage = "El estado no puede exceder los 50 caracteres")]
        public required string Estado { get; set; }

        public string? Observaciones { get; set; }
    }
} 