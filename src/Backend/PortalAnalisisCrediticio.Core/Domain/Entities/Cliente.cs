using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(20)]
        public string CUIT_CUIL { get; set; }

        [Required]
        [StringLength(20)]
        public string TipoDocumento { get; set; }

        [Required]
        [StringLength(200)]
        public string Direccion { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public bool Activo { get; set; }

        // Relación muchos a muchos con Compañía
        public ICollection<ClienteCompania> ClienteCompanias { get; set; }

        // Relaciones
        public InformacionFinanciera InformacionFinanciera { get; set; }
        public ICollection<ClienteEmpresa> ClienteEmpresas { get; set; }
        public ICollection<InformeRiesgo> InformesRiesgo { get; set; }
        public ICollection<SolicitudProducto> SolicitudesProducto { get; set; }
        public ICollection<InformeExterno> InformesExternos { get; set; }
        public ICollection<AnalisisCrediticio> AnalisisCrediticios { get; set; }
    }
} 