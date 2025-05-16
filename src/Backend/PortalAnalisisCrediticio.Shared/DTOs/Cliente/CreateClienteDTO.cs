using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Cliente
{
    public class CreateClienteDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El CUIT/CUIL es requerido")]
        [StringLength(20, ErrorMessage = "El CUIT/CUIL no puede exceder los 20 caracteres")]
        public required string CUIT_CUIL { get; set; }

        [Required(ErrorMessage = "El tipo de documento es requerido")]
        [StringLength(20, ErrorMessage = "El tipo de documento no puede exceder los 20 caracteres")]
        public required string TipoDocumento { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres")]
        public required string Direccion { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres")]
        public required string Email { get; set; }
    }
} 