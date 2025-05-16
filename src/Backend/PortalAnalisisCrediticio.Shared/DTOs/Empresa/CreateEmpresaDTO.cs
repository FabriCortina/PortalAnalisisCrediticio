using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Empresa
{
    public class CreateEmpresaDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El RUC es requerido")]
        [StringLength(20, ErrorMessage = "El RUC no puede exceder los 20 caracteres")]
        public required string Ruc { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres")]
        public string? Direccion { get; set; }
    }
} 