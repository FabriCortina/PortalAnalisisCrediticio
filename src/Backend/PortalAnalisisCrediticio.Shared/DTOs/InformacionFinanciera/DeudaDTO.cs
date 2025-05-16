using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.InformacionFinanciera
{
    public class DeudaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del cliente es requerido")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El tipo de deuda es requerido")]
        [StringLength(50, ErrorMessage = "El tipo de deuda no puede exceder los 50 caracteres")]
        public required string TipoDeuda { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La entidad acreedora es requerida")]
        [StringLength(100, ErrorMessage = "La entidad no puede exceder los 100 caracteres")]
        public required string EntidadAcreedora { get; set; }

        public DateTime? FechaVencimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
} 