using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities
{
    public class Actividad
    {
        [Key]
        public string Id { get; set; }
        
        [Required]
        public string UsuarioId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Accion { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Detalle { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string IpAddress { get; set; }
        
        [Required]
        public DateTime Fecha { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TipoActividad { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
    }
} 