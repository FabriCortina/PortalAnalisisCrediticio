using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities
{
    public class ClienteCompania
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int CompaniaId { get; set; }

        public DateTime FechaAsociacion { get; set; }
        public DateTime? FechaDesasociacion { get; set; }

        [StringLength(50)]
        public string Estado { get; set; }

        // Relaciones
        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }

        [ForeignKey("CompaniaId")]
        public Compania Compania { get; set; }
    }
} 