using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.Auditoria;

namespace PortalAnalisisCrediticio.Infrastructure.Services
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuditoriaService> _logger;
        private readonly IMemoryCache _cache;
        private const int CACHE_DURATION_MINUTES = 15;

        public AuditoriaService(
            ApplicationDbContext context,
            ILogger<AuditoriaService> logger,
            IMemoryCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        public async Task<AuditoriaDTO> RegistrarActividadAsync(AuditoriaDTO auditoria)
        {
            try
            {
                _logger.LogInformation($"Registrando actividad: {auditoria.Accion} - Usuario: {auditoria.UsuarioNombre}");

                var entidad = new Auditoria
                {
                    UsuarioId = auditoria.UsuarioId,
                    UsuarioNombre = auditoria.UsuarioNombre,
                    Accion = auditoria.Accion,
                    Detalle = auditoria.Detalle,
                    Fecha = DateTime.UtcNow,
                    IpAddress = auditoria.IpAddress,
                    UserAgent = auditoria.UserAgent,
                    EntidadAfectada = auditoria.EntidadAfectada,
                    EntidadId = auditoria.EntidadId
                };

                _context.Auditoria.Add(entidad);
                await _context.SaveChangesAsync();

                // Invalidar caché de actividades recientes
                _cache.Remove("actividades_recientes");

                _logger.LogInformation($"Actividad registrada exitosamente. ID: {entidad.Id}");
                return MapToDTO(entidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar actividad de auditoría");
                throw new Exception("Error al registrar actividad de auditoría", ex);
            }
        }

        public async Task<List<AuditoriaDTO>> ObtenerActividadesRecientesAsync(int cantidad = 10)
        {
            try
            {
                _logger.LogInformation($"Obteniendo {cantidad} actividades recientes");

                var cacheKey = $"actividades_recientes_{cantidad}";
                if (_cache.TryGetValue(cacheKey, out List<AuditoriaDTO> cachedActividades))
                {
                    _logger.LogInformation("Retornando actividades recientes desde caché");
                    return cachedActividades;
                }

                var actividades = await _context.Auditoria
                    .OrderByDescending(a => a.Fecha)
                    .Take(cantidad)
                    .Select(a => MapToDTO(a))
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, actividades, cacheOptions);
                _logger.LogInformation($"Se encontraron {actividades.Count} actividades recientes");

                return actividades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener actividades recientes");
                throw new Exception("Error al obtener actividades recientes", ex);
            }
        }

        public async Task<List<AuditoriaDTO>> ObtenerActividadesPorUsuarioAsync(string usuarioId, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                _logger.LogInformation($"Obteniendo actividades para el usuario {usuarioId}");

                var query = _context.Auditoria
                    .Where(a => a.UsuarioId == usuarioId);

                if (fechaInicio.HasValue)
                {
                    query = query.Where(a => a.Fecha >= fechaInicio.Value);
                }

                if (fechaFin.HasValue)
                {
                    query = query.Where(a => a.Fecha <= fechaFin.Value);
                }

                var actividades = await query
                    .OrderByDescending(a => a.Fecha)
                    .Select(a => MapToDTO(a))
                    .ToListAsync();

                _logger.LogInformation($"Se encontraron {actividades.Count} actividades para el usuario {usuarioId}");
                return actividades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener actividades para el usuario {usuarioId}");
                throw new Exception($"Error al obtener actividades para el usuario {usuarioId}", ex);
            }
        }

        public async Task<List<AuditoriaDTO>> ObtenerActividadesPorEntidadAsync(string entidadAfectada, string entidadId)
        {
            try
            {
                _logger.LogInformation($"Obteniendo actividades para la entidad {entidadAfectada} con ID {entidadId}");

                var actividades = await _context.Auditoria
                    .Where(a => a.EntidadAfectada == entidadAfectada && a.EntidadId == entidadId)
                    .OrderByDescending(a => a.Fecha)
                    .Select(a => MapToDTO(a))
                    .ToListAsync();

                _logger.LogInformation($"Se encontraron {actividades.Count} actividades para la entidad {entidadAfectada}");
                return actividades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener actividades para la entidad {entidadAfectada}");
                throw new Exception($"Error al obtener actividades para la entidad {entidadAfectada}", ex);
            }
        }

        public async Task<List<AuditoriaDTO>> ObtenerActividadesPorFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _logger.LogInformation($"Obteniendo actividades entre {fechaInicio:yyyy-MM-dd} y {fechaFin:yyyy-MM-dd}");

                var cacheKey = $"actividades_fecha_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}";
                if (_cache.TryGetValue(cacheKey, out List<AuditoriaDTO> cachedActividades))
                {
                    _logger.LogInformation("Retornando actividades por fecha desde caché");
                    return cachedActividades;
                }

                var actividades = await _context.Auditoria
                    .Where(a => a.Fecha >= fechaInicio && a.Fecha <= fechaFin)
                    .OrderByDescending(a => a.Fecha)
                    .Select(a => MapToDTO(a))
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, actividades, cacheOptions);
                _logger.LogInformation($"Se encontraron {actividades.Count} actividades en el período especificado");

                return actividades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener actividades por fecha");
                throw new Exception("Error al obtener actividades por fecha", ex);
            }
        }

        public async Task<List<AuditoriaDTO>> ObtenerActividadesPorAccionAsync(string accion)
        {
            try
            {
                _logger.LogInformation($"Obteniendo actividades con acción: {accion}");

                var cacheKey = $"actividades_accion_{accion}";
                if (_cache.TryGetValue(cacheKey, out List<AuditoriaDTO> cachedActividades))
                {
                    _logger.LogInformation("Retornando actividades por acción desde caché");
                    return cachedActividades;
                }

                var actividades = await _context.Auditoria
                    .Where(a => a.Accion == accion)
                    .OrderByDescending(a => a.Fecha)
                    .Select(a => MapToDTO(a))
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, actividades, cacheOptions);
                _logger.LogInformation($"Se encontraron {actividades.Count} actividades con la acción {accion}");

                return actividades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener actividades por acción {accion}");
                throw new Exception($"Error al obtener actividades por acción {accion}", ex);
            }
        }

        public async Task<AuditoriaDTO> ObtenerActividadPorIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Obteniendo actividad con ID: {id}");

                var cacheKey = $"actividad_{id}";
                if (_cache.TryGetValue(cacheKey, out AuditoriaDTO cachedActividad))
                {
                    _logger.LogInformation("Retornando actividad desde caché");
                    return cachedActividad;
                }

                var actividad = await _context.Auditoria
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (actividad == null)
                {
                    _logger.LogWarning($"No se encontró la actividad con ID: {id}");
                    return null;
                }

                var actividadDTO = MapToDTO(actividad);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, actividadDTO, cacheOptions);
                _logger.LogInformation($"Actividad encontrada: {actividad.Accion}");

                return actividadDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener actividad con ID {id}");
                throw new Exception($"Error al obtener actividad con ID {id}", ex);
            }
        }

        private static AuditoriaDTO MapToDTO(Auditoria auditoria)
        {
            return new AuditoriaDTO
            {
                Id = auditoria.Id,
                UsuarioId = auditoria.UsuarioId,
                UsuarioNombre = auditoria.UsuarioNombre,
                Accion = auditoria.Accion,
                Detalle = auditoria.Detalle,
                Fecha = auditoria.Fecha,
                IpAddress = auditoria.IpAddress,
                UserAgent = auditoria.UserAgent,
                EntidadAfectada = auditoria.EntidadAfectada,
                EntidadId = auditoria.EntidadId
            };
        }
    }
} 