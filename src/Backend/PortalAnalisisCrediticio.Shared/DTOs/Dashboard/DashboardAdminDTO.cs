using System.Collections.Generic;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard
{
    public class DashboardAdminDTO
    {
        public int TotalClientes { get; set; }
        public int TotalSolicitudes { get; set; }
        public int CreditosActivos { get; set; }
        public int CreditosVencidos { get; set; }
        public Dictionary<string, int> DistribucionRiesgos { get; set; }
    }
} 