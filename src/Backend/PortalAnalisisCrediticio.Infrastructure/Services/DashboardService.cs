using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

namespace PortalAnalisisCrediticio.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ILogger<DashboardService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const int CACHE_DURATION_MINUTES = 15;

        public DashboardService(
            ILogger<DashboardService> logger,
            ApplicationDbContext context,
            IMemoryCache cache)
        {
            _logger = logger;
            _context = context;
            _cache = cache;
        }

        public async Task<DashboardDTO> ObtenerDashboardAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando obtención de dashboard");

                var cacheKey = "dashboard_principal";
                if (_cache.TryGetValue(cacheKey, out DashboardDTO cachedDashboard))
                {
                    _logger.LogInformation("Retornando dashboard desde caché");
                    return cachedDashboard;
                }

                var metricas = await ObtenerMetricasAsync();
                var creditosActivos = await ObtenerCreditosActivosAsync();
                var clientesRiesgo = await ObtenerClientesRiesgoAsync();
                var creditosVencidos = await ObtenerCreditosVencidosAsync();
                var actividadesRecientes = await ObtenerActividadesRecientesAsync();

                var dashboard = new DashboardDTO
                {
                    Metricas = metricas,
                    CreditosActivos = creditosActivos,
                    ClientesRiesgo = clientesRiesgo,
                    CreditosVencidos = creditosVencidos,
                    ActividadesRecientes = actividadesRecientes
                };

                _cache.Set(cacheKey, dashboard, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                _logger.LogInformation("Dashboard generado y almacenado en caché");

                return dashboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener dashboard");
                throw;
            }
        }

        public async Task<List<CreditoActivoDTO>> ObtenerCreditosActivosAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando obtención de créditos activos");

                var cacheKey = "creditos_activos";
                if (_cache.TryGetValue(cacheKey, out List<CreditoActivoDTO> cachedCreditos))
                {
                    _logger.LogInformation("Retornando créditos activos desde caché");
                    return cachedCreditos;
                }

                var creditos = await _context.Creditos
                    .Include(c => c.Cliente)
                    .Where(c => c.Estado == "Activo")
                    .OrderByDescending(c => c.FechaCreacion)
                    .Take(10)
                    .Select(c => new CreditoActivoDTO
                    {
                        Id = c.Id,
                        ClienteNombre = c.Cliente.NombreCompleto,
                        Monto = c.Monto,
                        FechaCreacion = c.FechaCreacion,
                        Estado = c.Estado,
                        TipoCredito = c.TipoCredito
                    })
                    .ToListAsync();

                _cache.Set(cacheKey, creditos, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                _logger.LogInformation($"Se encontraron {creditos.Count} créditos activos");

                return creditos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener créditos activos");
                throw;
            }
        }

        public async Task<List<ClienteRiesgoDTO>> ObtenerClientesRiesgoAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando obtención de clientes en riesgo");

                var cacheKey = "clientes_riesgo";
                if (_cache.TryGetValue(cacheKey, out List<ClienteRiesgoDTO> cachedClientes))
                {
                    _logger.LogInformation("Retornando clientes en riesgo desde caché");
                    return cachedClientes;
                }

                var clientes = await _context.Clientes
                    .Include(c => c.Creditos)
                    .Where(c => c.NivelRiesgo == "Alto" || c.NivelRiesgo == "Medio")
                    .OrderByDescending(c => c.FechaActualizacion)
                    .Take(10)
                    .Select(c => new ClienteRiesgoDTO
                    {
                        Id = c.Id,
                        NombreCompleto = c.NombreCompleto,
                        NivelRiesgo = c.NivelRiesgo,
                        MontoTotalCredito = c.Creditos.Sum(cr => cr.Monto),
                        CreditosActivos = c.Creditos.Count(cr => cr.Estado == "Activo"),
                        FechaActualizacion = c.FechaActualizacion
                    })
                    .ToListAsync();

                _cache.Set(cacheKey, clientes, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                _logger.LogInformation($"Se encontraron {clientes.Count} clientes en riesgo");

                return clientes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clientes en riesgo");
                throw;
            }
        }

        public async Task<List<CreditoVencidoDTO>> ObtenerCreditosVencidosAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando obtención de créditos vencidos");

                var cacheKey = "creditos_vencidos";
                if (_cache.TryGetValue(cacheKey, out List<CreditoVencidoDTO> cachedCreditos))
                {
                    _logger.LogInformation("Retornando créditos vencidos desde caché");
                    return cachedCreditos;
                }

                var creditos = await _context.Creditos
                    .Include(c => c.Cliente)
                    .Where(c => c.Estado == "Vencido")
                    .OrderByDescending(c => c.FechaVencimiento)
                    .Take(10)
                    .Select(c => new CreditoVencidoDTO
                    {
                        Id = c.Id,
                        ClienteNombre = c.Cliente.NombreCompleto,
                        Monto = c.Monto,
                        FechaVencimiento = c.FechaVencimiento,
                        DiasVencido = (DateTime.UtcNow - c.FechaVencimiento).Days,
                        Estado = c.Estado
                    })
                    .ToListAsync();

                _cache.Set(cacheKey, creditos, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                _logger.LogInformation($"Se encontraron {creditos.Count} créditos vencidos");

                return creditos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener créditos vencidos");
                throw;
            }
        }

        public async Task<MetricasDTO> ObtenerMetricasAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando obtención de métricas");

                var cacheKey = "metricas";
                if (_cache.TryGetValue(cacheKey, out MetricasDTO cachedMetricas))
                {
                    _logger.LogInformation("Retornando métricas desde caché");
                    return cachedMetricas;
                }

                var totalClientes = await _context.Clientes.CountAsync();
                var totalCreditos = await _context.Creditos.CountAsync();
                var montoTotalPrestado = await _context.Creditos.SumAsync(c => c.Monto);
                var creditosActivos = await _context.Creditos.CountAsync(c => c.Estado == "Activo");
                var creditosVencidos = await _context.Creditos.CountAsync(c => c.Estado == "Vencido");
                var montoVencido = await _context.Creditos
                    .Where(c => c.Estado == "Vencido")
                    .SumAsync(c => c.Monto);

                var metricas = new MetricasDTO
                {
                    TotalClientes = totalClientes,
                    TotalCreditos = totalCreditos,
                    MontoTotalPrestado = montoTotalPrestado,
                    CreditosActivos = creditosActivos,
                    CreditosVencidos = creditosVencidos,
                    MontoVencido = montoVencido,
                    TasaVencimiento = totalCreditos > 0 ? (decimal)creditosVencidos / totalCreditos * 100 : 0
                };

                _cache.Set(cacheKey, metricas, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                _logger.LogInformation("Métricas generadas y almacenadas en caché");

                return metricas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener métricas");
                throw;
            }
        }

        public async Task<List<ActividadDTO>> ObtenerActividadesRecientesAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo actividades recientes");
                var cacheKey = "actividades_recientes";

                if (_cache.TryGetValue(cacheKey, out List<ActividadDTO> cachedActividades))
                {
                    _logger.LogInformation("Retornando actividades recientes desde caché");
                    return cachedActividades;
                }

                var actividades = await _context.Actividades
                    .OrderByDescending(a => a.Fecha)
                    .Take(10)
                    .Select(a => new ActividadDTO
                    {
                        Id = a.Id,
                        Usuario = a.Usuario,
                        Accion = a.Accion,
                        Entidad = a.Entidad,
                        Detalles = a.Detalles,
                        Fecha = a.Fecha
                    })
                    .ToListAsync();

                _cache.Set(cacheKey, actividades, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                _logger.LogInformation($"Se obtuvieron {actividades.Count} actividades recientes");
                return actividades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener actividades recientes");
                throw;
            }
        }

        public async Task<DashboardAdminDTO> ObtenerDashboardAdminAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando obtención de dashboard administrativo");

                var cacheKey = "dashboard_admin";
                if (_cache.TryGetValue(cacheKey, out DashboardAdminDTO cachedDashboard))
                {
                    _logger.LogInformation("Retornando dashboard administrativo desde caché");
                    return cachedDashboard;
                }

                var totalClientes = await _context.Clientes.CountAsync();
                var totalSolicitudes = await _context.Solicitudes.CountAsync();
                var creditosActivos = await _context.Creditos.CountAsync(c => c.Estado == "Activo");
                var creditosVencidos = await _context.Creditos.CountAsync(c => c.Estado == "Vencido");
                var distribucionRiesgos = await _context.Clientes
                    .GroupBy(c => c.NivelRiesgo)
                    .Select(g => new { NivelRiesgo = g.Key, Cantidad = g.Count() })
                    .ToListAsync();

                var dashboardAdmin = new DashboardAdminDTO
                {
                    TotalClientes = totalClientes,
                    TotalSolicitudes = totalSolicitudes,
                    CreditosActivos = creditosActivos,
                    CreditosVencidos = creditosVencidos,
                    DistribucionRiesgos = distribucionRiesgos.ToDictionary(d => d.NivelRiesgo, d => d.Cantidad)
                };

                _cache.Set(cacheKey, dashboardAdmin, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
                _logger.LogInformation("Dashboard administrativo generado y almacenado en caché");

                return dashboardAdmin;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener dashboard administrativo");
                throw;
            }
        }
    }
} 