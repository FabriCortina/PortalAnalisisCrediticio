using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.ClienteCompania
{
    public class UpdateClienteCompaniaDTO
    {
        [Required(ErrorMessage = "El ID del cliente es requerido")]
        public required int ClienteId { get; set; }

        [Required(ErrorMessage = "El ID de la compañía es requerido")]
        public required int CompaniaId { get; set; }

        [Required(ErrorMessage = "La fecha de asociación es requerida")]
        public required DateTime FechaAsociacion { get; set; }

        public DateTime? FechaDesasociacion { get; set; }

        [StringLength(50, ErrorMessage = "El estado no puede exceder los 50 caracteres")]
        public string? Estado { get; set; }
    }
} 