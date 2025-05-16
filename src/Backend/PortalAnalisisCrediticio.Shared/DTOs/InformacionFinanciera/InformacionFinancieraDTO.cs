using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.InformacionFinanciera
{
    public class InformacionFinancieraDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del cliente es requerido")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "La moneda es requerida")]
        [StringLength(3, ErrorMessage = "La moneda debe tener 3 caracteres")]
        public required string Moneda { get; set; }

        [Required(ErrorMessage = "La fuente de información es requerida")]
        [StringLength(100, ErrorMessage = "La fuente de información no puede exceder los 100 caracteres")]
        public required string FuenteInformacion { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
        public string? Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        // Relaciones
        public ICollection<EstadoFinancieroDTO> EstadosFinancieros { get; set; } = new List<EstadoFinancieroDTO>();
        public ICollection<FlujoCajaProyectadoDTO> FlujosCajaProyectados { get; set; } = new List<FlujoCajaProyectadoDTO>();
        public ICollection<DeudaDTO> Deudas { get; set; } = new List<DeudaDTO>();
    }
} 