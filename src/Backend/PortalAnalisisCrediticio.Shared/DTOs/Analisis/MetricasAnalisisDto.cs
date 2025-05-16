namespace PortalAnalisisCrediticio.Shared.DTOs
{
    public class MetricasAnalisisDto
    {
        public MetricaDto TotalAnalisis { get; set; }
        public MetricaDto ClientesActivos { get; set; }
        public MetricaDto MontoTotal { get; set; }
        public MetricaDto AlertasActivas { get; set; }
    }

    public class MetricaDto
    {
        public string Valor { get; set; }
        public double Tendencia { get; set; }
    }
} 