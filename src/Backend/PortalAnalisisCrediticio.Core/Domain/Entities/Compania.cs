using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities
{
    public class Compania
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(20)]
        public string Codigo { get; set; }

        [Required]
        [StringLength(3)]
        public string MonedaPrincipal { get; set; }

        [Required]
        [StringLength(100)]
        public string Pais { get; set; }

        [Required]
        [StringLength(50)]
        public string ConfiguracionRegional { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        // Relación muchos a muchos con Cliente
        public ICollection<ClienteCompania> ClienteCompanias { get; set; }
    }
} 