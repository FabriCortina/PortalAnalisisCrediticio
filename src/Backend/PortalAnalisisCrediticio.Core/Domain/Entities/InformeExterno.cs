using System;

namespace PortalAnalisisCrediticio.Core.Domain.Entities
{
    public class InformeExterno
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Fuente { get; set; }
        public string TipoInforme { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaInforme { get; set; }
        public string Estado { get; set; }
        public Cliente Cliente { get; set; }
    }
} 