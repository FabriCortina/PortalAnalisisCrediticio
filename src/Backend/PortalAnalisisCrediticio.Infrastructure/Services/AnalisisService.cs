using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Infrastructure.Services
{
    public class AnalisisService : IAnalisisService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AnalisisService> _logger;

        public AnalisisService(ApplicationDbContext context, ILogger<AnalisisService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MetricasAnalisisDto> GetMetricasAsync(int periodo)
        {
            var fechaInicio = DateTime.UtcNow.AddDays(-periodo);
            var fechaFin = DateTime.UtcNow;

            // Obtener métricas del período actual
            var totalAnalisisActual = await _context.AnalisisCrediticios
                .CountAsync(a => a.FechaAnalisis >= fechaInicio && a.FechaAnalisis <= fechaFin);

            var clientesActivosActual = await _context.Clientes
                .CountAsync(c => c.UltimaActualizacion >= fechaInicio && c.UltimaActualizacion <= fechaFin);

            var montoTotalActual = await _context.AnalisisCrediticios
                .Where(a => a.FechaAnalisis >= fechaInicio && a.FechaAnalisis <= fechaFin)
                .SumAsync(a => a.MontoSolicitado);

            var alertasActivasActual = await _context.Alertas
                .CountAsync(a => a.FechaCreacion >= fechaInicio && a.FechaCreacion <= fechaFin && !a.Resuelta);

            // Obtener métricas del período anterior
            var fechaInicioAnterior = fechaInicio.AddDays(-periodo);
            var fechaFinAnterior = fechaInicio;

            var totalAnalisisAnterior = await _context.AnalisisCrediticios
                .CountAsync(a => a.FechaAnalisis >= fechaInicioAnterior && a.FechaAnalisis <= fechaFinAnterior);

            var clientesActivosAnterior = await _context.Clientes
                .CountAsync(c => c.UltimaActualizacion >= fechaInicioAnterior && c.UltimaActualizacion <= fechaFinAnterior);

            var montoTotalAnterior = await _context.AnalisisCrediticios
                .Where(a => a.FechaAnalisis >= fechaInicioAnterior && a.FechaAnalisis <= fechaFinAnterior)
                .SumAsync(a => a.MontoSolicitado);

            var alertasActivasAnterior = await _context.Alertas
                .CountAsync(a => a.FechaCreacion >= fechaInicioAnterior && a.FechaCreacion <= fechaFinAnterior && !a.Resuelta);

            // Calcular tendencias
            double CalcularTendencia(double actual, double anterior)
            {
                if (anterior == 0) return 100;
                return ((actual - anterior) / anterior) * 100;
            }

            return new MetricasAnalisisDto
            {
                TotalAnalisis = new MetricaDto
                {
                    Valor = totalAnalisisActual.ToString("N0"),
                    Tendencia = CalcularTendencia(totalAnalisisActual, totalAnalisisAnterior)
                },
                ClientesActivos = new MetricaDto
                {
                    Valor = clientesActivosActual.ToString("N0"),
                    Tendencia = CalcularTendencia(clientesActivosActual, clientesActivosAnterior)
                },
                MontoTotal = new MetricaDto
                {
                    Valor = $"${montoTotalActual:N2}M",
                    Tendencia = CalcularTendencia(montoTotalActual, montoTotalAnterior)
                },
                AlertasActivas = new MetricaDto
                {
                    Valor = alertasActivasActual.ToString("N0"),
                    Tendencia = CalcularTendencia(alertasActivasActual, alertasActivasAnterior)
                }
            };
        }

        public async Task<DistribucionRiesgosDto> GetDistribucionRiesgosAsync(int periodo)
        {
            var fechaInicio = DateTime.UtcNow.AddDays(-periodo);
            var fechaFin = DateTime.UtcNow;

            var distribucion = await _context.AnalisisCrediticios
                .Where(a => a.FechaAnalisis >= fechaInicio && a.FechaAnalisis <= fechaFin)
                .GroupBy(a => a.NivelRiesgo)
                .Select(g => new { NivelRiesgo = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            var series = new int[3]; // [Bajo, Medio, Alto]
            foreach (var item in distribucion)
            {
                switch (item.NivelRiesgo.ToLower())
                {
                    case "bajo":
                        series[0] = item.Cantidad;
                        break;
                    case "medio":
                        series[1] = item.Cantidad;
                        break;
                    case "alto":
                        series[2] = item.Cantidad;
                        break;
                }
            }

            return new DistribucionRiesgosDto { Series = series };
        }

        public async Task<TendenciasCreditoDto> GetTendenciasCreditoAsync(int periodo)
        {
            var fechaInicio = DateTime.UtcNow.AddDays(-periodo);
            var fechaFin = DateTime.UtcNow;

            var tendencias = await _context.AnalisisCrediticios
                .Where(a => a.FechaAnalisis >= fechaInicio && a.FechaAnalisis <= fechaFin)
                .GroupBy(a => new { a.FechaAnalisis.Year, a.FechaAnalisis.Month })
                .Select(g => new
                {
                    Fecha = new DateTime(g.Key.Year, g.Key.Month, 1),
                    MontoTotal = g.Sum(a => a.MontoSolicitado)
                })
                .OrderBy(x => x.Fecha)
                .ToListAsync();

            return new TendenciasCreditoDto
            {
                Valores = tendencias.Select(t => t.MontoTotal).ToArray(),
                Categorias = tendencias.Select(t => t.Fecha.ToString("MMM")).ToArray()
            };
        }

        public async Task<IEnumerable<UltimoAnalisisDto>> GetUltimosAnalisisAsync()
        {
            return await _context.AnalisisCrediticios
                .OrderByDescending(a => a.FechaAnalisis)
                .Take(10)
                .Select(a => new UltimoAnalisisDto
                {
                    Id = a.Id,
                    Cliente = a.Cliente.Nombre,
                    Fecha = a.FechaAnalisis.ToString("yyyy-MM-dd"),
                    Riesgo = a.NivelRiesgo,
                    Estado = a.Estado
                })
                .ToListAsync();
        }
    }
} 