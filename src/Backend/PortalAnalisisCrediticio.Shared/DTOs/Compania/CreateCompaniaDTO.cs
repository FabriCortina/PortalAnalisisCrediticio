using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Compania
{
    public class CreateCompaniaDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El código es requerido")]
        [StringLength(20, ErrorMessage = "El código no puede exceder los 20 caracteres")]
        public required string Codigo { get; set; }

        [Required(ErrorMessage = "La moneda principal es requerida")]
        [StringLength(3, ErrorMessage = "La moneda principal debe tener 3 caracteres")]
        public required string MonedaPrincipal { get; set; }

        [Required(ErrorMessage = "El país es requerido")]
        [StringLength(100, ErrorMessage = "El país no puede exceder los 100 caracteres")]
        public required string Pais { get; set; }

        [Required(ErrorMessage = "La configuración regional es requerida")]
        [StringLength(10, ErrorMessage = "La configuración regional no puede exceder los 10 caracteres")]
        public required string ConfiguracionRegional { get; set; }
    }
} 