using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.InformacionFinanciera
{
    public class EstadoFinancieroDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del cliente es requerido")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El año es requerido")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "El ingreso es requerido")]
        public decimal Ingreso { get; set; }

        [Required(ErrorMessage = "El egreso es requerido")]
        public decimal Egreso { get; set; }

        [Required(ErrorMessage = "El activo es requerido")]
        public decimal Activo { get; set; }

        [Required(ErrorMessage = "El pasivo es requerido")]
        public decimal Pasivo { get; set; }

        public decimal? PatrimonioNeto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
} 