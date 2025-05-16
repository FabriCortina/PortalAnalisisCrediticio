using System;

namespace PortalAnalisisCrediticio.Core.Domain.Entities
{
    public class Alerta
    {
        public int Id { get; set; }
        public int AnalisisCrediticioId { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public string Nivel { get; set; }
        public bool Resuelta { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaResolucion { get; set; }

        // Relaciones
        public AnalisisCrediticio AnalisisCrediticio { get; set; }
    }
} 