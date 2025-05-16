using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities
{
    public class Notificacion
    {
        [Key]
        public string Id { get; set; }
        
        [Required]
        public string UsuarioId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Mensaje { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Tipo { get; set; }
        
        [Required]
        public DateTime FechaCreacion { get; set; }
        
        public DateTime? FechaLectura { get; set; }
        
        [Required]
        public bool Leida { get; set; }
        
        [MaxLength(500)]
        public string Enlace { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
    }
} 