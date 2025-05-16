using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.InformacionFinanciera
{
    public class FlujoCajaProyectadoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del cliente es requerido")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El año es requerido")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "El mes es requerido")]
        public int Mes { get; set; }

        [Required(ErrorMessage = "El ingreso proyectado es requerido")]
        public decimal IngresoProyectado { get; set; }

        [Required(ErrorMessage = "El egreso proyectado es requerido")]
        public decimal EgresoProyectado { get; set; }

        public decimal? SaldoFinal { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
} 