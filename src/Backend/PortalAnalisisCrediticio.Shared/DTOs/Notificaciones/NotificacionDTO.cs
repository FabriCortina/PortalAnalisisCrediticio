using System;

namespace PortalAnalisisCrediticio.Shared.DTOs.Notificaciones
{
    public class NotificacionDTO
    {
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaLectura { get; set; }
        public bool Leida { get; set; }
        public string Enlace { get; set; }
    }

    public class NotificacionRequestDTO
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string Tipo { get; set; }
        public string Enlace { get; set; }
        public List<string> UsuariosDestino { get; set; }
    }
} 