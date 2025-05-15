using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Core.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; }

        [Required]
        [MaxLength(11)]
        public string CUIT { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Telefono { get; set; }

        // Relaciones
        public InformacionFinanciera InformacionFinanciera { get; set; }
        public ICollection<ClienteEmpresa> ClienteEmpresas { get; set; }
        public ICollection<InformeRiesgo> InformesRiesgo { get; set; }
        public ICollection<SolicitudProducto> SolicitudesProducto { get; set; }
        public ICollection<InformeExterno> InformesExternos { get; set; }
    }
} 