using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.Integraciones;
using System.Net.Http;
using System.Text.Json;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class IntegracionesExternasService : IIntegracionesExternasService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IntegracionesExternasService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public IntegracionesExternasService(
        ApplicationDbContext context,
        ILogger<IntegracionesExternasService> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<InformeNosisDTO> GetInformeNosisAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Obteniendo informe Nosis para el cliente {clienteId}");

            // Verificar si existe un informe reciente (menos de 24 horas)
            var informeReciente = await _context.InformesExternos
                .Where(i => i.ClienteId == clienteId && 
                           i.Fuente == "Nosis" && 
                           i.FechaConsulta >= DateTime.UtcNow.AddHours(-24))
                .OrderByDescending(i => i.FechaConsulta)
                .FirstOrDefaultAsync();

            if (informeReciente != null)
            {
                _logger.LogInformation($"Se encontró un informe Nosis reciente para el cliente {clienteId}");
                return MapearInformeNosis(informeReciente);
            }

            // Obtener datos del cliente
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {clienteId} no encontrado para informe Nosis");
                throw new Exception($"Cliente {clienteId} no encontrado");
            }

            // Generar datos mock
            var informe = new InformeNosisDTO
            {
                Estado = "Activo",
                Score = "A",
                Riesgo = "Bajo",
                UltimaActualizacion = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Observaciones = "Sin incidencias registradas",
                Deudas = new List<DeudaNosisDTO>
                {
                    new DeudaNosisDTO
                    {
                        Entidad = "Banco Nación",
                        TipoDeuda = "Préstamo Personal",
                        Monto = 500000,
                        FechaVencimiento = DateTime.UtcNow.AddMonths(12),
                        Estado = "Al día"
                    }
                },
                Protestos = new List<ProtestoNosisDTO>(),
                SituacionCrediticia = "Normal",
                MontoTotalDeuda = 500000,
                CantidadDeudas = 1,
                CategoriaRiesgo = "A",
                Cheques = new List<ChequeNosisDTO>
                {
                    new ChequeNosisDTO
                    {
                        Banco = "Banco Nación",
                        NumeroCheque = "12345678",
                        FechaEmision = DateTime.UtcNow.AddDays(-30),
                        Monto = 100000,
                        Estado = "Cobrado"
                    }
                }
            };

            // Guardar informe
            await GuardarInformeAsync(clienteId, "Nosis", informe);

            return informe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener informe Nosis para el cliente {clienteId}");
            throw new Exception($"Error al obtener informe Nosis: {ex.Message}", ex);
        }
    }

    public async Task<InformeVerazDTO> GetInformeVerazAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Obteniendo informe Veraz para el cliente {clienteId}");

            var informeReciente = await _context.InformesExternos
                .Where(i => i.ClienteId == clienteId && 
                           i.Fuente == "Veraz" && 
                           i.FechaConsulta >= DateTime.UtcNow.AddHours(-24))
                .OrderByDescending(i => i.FechaConsulta)
                .FirstOrDefaultAsync();

            if (informeReciente != null)
            {
                _logger.LogInformation($"Se encontró un informe Veraz reciente para el cliente {clienteId}");
                return MapearInformeVeraz(informeReciente);
            }

            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {clienteId} no encontrado para informe Veraz");
                throw new Exception($"Cliente {clienteId} no encontrado");
            }

            // Generar datos mock
            var informe = new InformeVerazDTO
            {
                Estado = "Activo",
                Score = "B",
                Riesgo = "Medio",
                UltimaActualizacion = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Observaciones = "Sin incidencias registradas",
                Deudas = new List<DeudaVerazDTO>
                {
                    new DeudaVerazDTO
                    {
                        Entidad = "Banco Galicia",
                        TipoDeuda = "Tarjeta de Crédito",
                        Monto = 250000,
                        FechaVencimiento = DateTime.UtcNow.AddMonths(6),
                        Estado = "Al día"
                    }
                },
                Consultas = new List<ConsultaVerazDTO>
                {
                    new ConsultaVerazDTO
                    {
                        Entidad = "Banco Galicia",
                        FechaConsulta = DateTime.UtcNow.AddDays(-15),
                        TipoConsulta = "Préstamo Personal",
                        Resultado = "Aprobado"
                    }
                },
                SituacionCrediticia = "Normal",
                MontoTotalDeuda = 250000,
                CantidadDeudas = 1,
                CategoriaRiesgo = "B"
            };

            await GuardarInformeAsync(clienteId, "Veraz", informe);

            return informe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener informe Veraz para el cliente {clienteId}");
            throw new Exception($"Error al obtener informe Veraz: {ex.Message}", ex);
        }
    }

    public async Task<InformeInfoexpertoDTO> GetInformeInfoexpertoAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Obteniendo informe Infoexperto para el cliente {clienteId}");

            var informeReciente = await _context.InformesExternos
                .Where(i => i.ClienteId == clienteId && 
                           i.Fuente == "Infoexperto" && 
                           i.FechaConsulta >= DateTime.UtcNow.AddHours(-24))
                .OrderByDescending(i => i.FechaConsulta)
                .FirstOrDefaultAsync();

            if (informeReciente != null)
            {
                _logger.LogInformation($"Se encontró un informe Infoexperto reciente para el cliente {clienteId}");
                return MapearInformeInfoexperto(informeReciente);
            }

            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {clienteId} no encontrado para informe Infoexperto");
                throw new Exception($"Cliente {clienteId} no encontrado");
            }

            var informe = await ConsultarAPIExternaAsync<InformeInfoexpertoDTO>(
                "Infoexperto",
                cliente.Documento,
                "infoexperto-api-endpoint");

            await GuardarInformeAsync(clienteId, "Infoexperto", informe);

            return informe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener informe Infoexperto para el cliente {clienteId}");
            throw new Exception($"Error al obtener informe Infoexperto: {ex.Message}", ex);
        }
    }

    public async Task<InformeBCRADTO> GetInformeBCRAAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Obteniendo informe BCRA para el cliente {clienteId}");

            var informeReciente = await _context.InformesExternos
                .Where(i => i.ClienteId == clienteId && 
                           i.Fuente == "BCRA" && 
                           i.FechaConsulta >= DateTime.UtcNow.AddHours(-24))
                .OrderByDescending(i => i.FechaConsulta)
                .FirstOrDefaultAsync();

            if (informeReciente != null)
            {
                _logger.LogInformation($"Se encontró un informe BCRA reciente para el cliente {clienteId}");
                return MapearInformeBCRA(informeReciente);
            }

            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {clienteId} no encontrado para informe BCRA");
                throw new Exception($"Cliente {clienteId} no encontrado");
            }

            // Generar datos mock
            var informe = new InformeBCRADTO
            {
                Estado = "Activo",
                Score = "A",
                Riesgo = "Bajo",
                UltimaActualizacion = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Observaciones = "Sin incidencias registradas",
                Deudas = new List<DeudaBCRADTO>
                {
                    new DeudaBCRADTO
                    {
                        Entidad = "Banco Santander",
                        TipoDeuda = "Préstamo Hipotecario",
                        Monto = 15000000,
                        FechaVencimiento = DateTime.UtcNow.AddYears(20),
                        Estado = "Al día",
                        ClasificacionRiesgo = "Normal"
                    }
                },
                Garantias = new List<GarantiaBCRADTO>
                {
                    new GarantiaBCRADTO
                    {
                        TipoGarantia = "Hipoteca",
                        ValorGarantia = 20000000,
                        FechaConstitucion = DateTime.UtcNow.AddYears(-1),
                        Estado = "Activa",
                        EntidadGarante = "Banco Santander"
                    }
                },
                SituacionCrediticia = "Normal",
                MontoTotalDeuda = 15000000,
                CantidadDeudas = 1,
                CategoriaRiesgo = "A",
                Operaciones = new List<OperacionBCRADTO>
                {
                    new OperacionBCRADTO
                    {
                        TipoOperacion = "Préstamo Hipotecario",
                        FechaOperacion = DateTime.UtcNow.AddYears(-1),
                        Monto = 15000000,
                        Estado = "Activo",
                        Entidad = "Banco Santander"
                    }
                }
            };

            await GuardarInformeAsync(clienteId, "BCRA", informe);

            return informe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener informe BCRA para el cliente {clienteId}");
            throw new Exception($"Error al obtener informe BCRA: {ex.Message}", ex);
        }
    }

    public async Task<InformeAFIPDTO> GetInformeAFIPAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Obteniendo informe AFIP para el cliente {clienteId}");

            var informeReciente = await _context.InformesExternos
                .Where(i => i.ClienteId == clienteId && 
                           i.Fuente == "AFIP" && 
                           i.FechaConsulta >= DateTime.UtcNow.AddHours(-24))
                .OrderByDescending(i => i.FechaConsulta)
                .FirstOrDefaultAsync();

            if (informeReciente != null)
            {
                _logger.LogInformation($"Se encontró un informe AFIP reciente para el cliente {clienteId}");
                return MapearInformeAFIP(informeReciente);
            }

            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {clienteId} no encontrado para informe AFIP");
                throw new Exception($"Cliente {clienteId} no encontrado");
            }

            // Generar datos mock
            var informe = new InformeAFIPDTO
            {
                Estado = "Activo",
                Score = "A",
                Riesgo = "Bajo",
                UltimaActualizacion = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Observaciones = "Sin incidencias registradas",
                Impuestos = new List<ImpuestoAFIPDTO>
                {
                    new ImpuestoAFIPDTO
                    {
                        TipoImpuesto = "IVA",
                        Periodo = "2024-01",
                        Monto = 500000,
                        FechaVencimiento = DateTime.UtcNow.AddDays(15),
                        Estado = "Al día"
                    }
                },
                Retenciones = new List<RetencionAFIPDTO>
                {
                    new RetencionAFIPDTO
                    {
                        TipoRetencion = "Ganancias",
                        Periodo = "2024-01",
                        Monto = 100000,
                        FechaRetencion = DateTime.UtcNow.AddDays(-15),
                        Estado = "Procesada"
                    }
                },
                SituacionFiscal = "Regular",
                MontoTotalImpuestos = 500000,
                CantidadImpuestos = 1,
                CategoriaRiesgo = "A",
                Declaraciones = new List<DeclaracionAFIPDTO>
                {
                    new DeclaracionAFIPDTO
                    {
                        TipoDeclaracion = "IVA",
                        Periodo = "2024-01",
                        FechaPresentacion = DateTime.UtcNow.AddDays(-5),
                        Estado = "Presentada",
                        MontoDeclarado = 500000
                    }
                }
            };

            await GuardarInformeAsync(clienteId, "AFIP", informe);

            return informe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener informe AFIP para el cliente {clienteId}");
            throw new Exception($"Error al obtener informe AFIP: {ex.Message}", ex);
        }
    }

    public async Task<InformePublicoDTO> GetInformePublicoAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Obteniendo informe público para el cliente {clienteId}");

            var informeReciente = await _context.InformesExternos
                .Where(i => i.ClienteId == clienteId && 
                           i.Fuente == "Publico" && 
                           i.FechaConsulta >= DateTime.UtcNow.AddHours(-24))
                .OrderByDescending(i => i.FechaConsulta)
                .FirstOrDefaultAsync();

            if (informeReciente != null)
            {
                _logger.LogInformation($"Se encontró un informe público reciente para el cliente {clienteId}");
                return MapearInformePublico(informeReciente);
            }

            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {clienteId} no encontrado para informe público");
                throw new Exception($"Cliente {clienteId} no encontrado");
            }

            var informe = await ConsultarAPIExternaAsync<InformePublicoDTO>(
                "Publico",
                cliente.Documento,
                "publico-api-endpoint");

            await GuardarInformeAsync(clienteId, "Publico", informe);

            return informe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener informe público para el cliente {clienteId}");
            throw new Exception($"Error al obtener informe público: {ex.Message}", ex);
        }
    }

    private async Task<T> ConsultarAPIExternaAsync<T>(string fuente, string documento, string endpointKey) where T : class
    {
        try
        {
            var endpoint = _configuration[endpointKey];
            if (string.IsNullOrEmpty(endpoint))
            {
                _logger.LogWarning($"Endpoint no configurado para {fuente}");
                return Activator.CreateInstance<T>();
            }

            var client = _httpClientFactory.CreateClient(fuente);
            var response = await client.GetAsync($"{endpoint}?documento={documento}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content);
            }

            _logger.LogWarning($"Error al consultar {fuente}: {response.StatusCode}");
            return Activator.CreateInstance<T>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al consultar API de {fuente}");
            return Activator.CreateInstance<T>();
        }
    }

    private async Task GuardarInformeAsync<T>(int clienteId, string fuente, T informe) where T : class
    {
        try
        {
            var informeExterno = new InformeExterno
            {
                ClienteId = clienteId,
                Fuente = fuente,
                Estado = (string)informe.GetType().GetProperty("Estado").GetValue(informe),
                Score = (string)informe.GetType().GetProperty("Score").GetValue(informe),
                Riesgo = (string)informe.GetType().GetProperty("Riesgo").GetValue(informe),
                UltimaActualizacion = (string)informe.GetType().GetProperty("UltimaActualizacion").GetValue(informe),
                Observaciones = (string)informe.GetType().GetProperty("Observaciones").GetValue(informe),
                FechaConsulta = DateTime.UtcNow
            };

            _context.InformesExternos.Add(informeExterno);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Informe de {fuente} guardado exitosamente para el cliente {clienteId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al guardar informe de {fuente} para el cliente {clienteId}");
            throw;
        }
    }

    private InformeNosisDTO MapearInformeNosis(InformeExterno informe)
    {
        return new InformeNosisDTO
        {
            Estado = informe.Estado,
            Score = informe.Score,
            Riesgo = informe.Riesgo,
            UltimaActualizacion = informe.UltimaActualizacion,
            Observaciones = informe.Observaciones
        };
    }

    private InformeVerazDTO MapearInformeVeraz(InformeExterno informe)
    {
        return new InformeVerazDTO
        {
            Estado = informe.Estado,
            Score = informe.Score,
            Riesgo = informe.Riesgo,
            UltimaActualizacion = informe.UltimaActualizacion,
            Observaciones = informe.Observaciones
        };
    }

    private InformeInfoexpertoDTO MapearInformeInfoexperto(InformeExterno informe)
    {
        return new InformeInfoexpertoDTO
        {
            Estado = informe.Estado,
            Score = informe.Score,
            Riesgo = informe.Riesgo,
            UltimaActualizacion = informe.UltimaActualizacion,
            Observaciones = informe.Observaciones
        };
    }

    private InformeBCRADTO MapearInformeBCRA(InformeExterno informe)
    {
        return new InformeBCRADTO
        {
            Estado = informe.Estado,
            Score = informe.Score,
            Riesgo = informe.Riesgo,
            UltimaActualizacion = informe.UltimaActualizacion,
            Observaciones = informe.Observaciones
        };
    }

    private InformeAFIPDTO MapearInformeAFIP(InformeExterno informe)
    {
        return new InformeAFIPDTO
        {
            Estado = informe.Estado,
            Score = informe.Score,
            Riesgo = informe.Riesgo,
            UltimaActualizacion = informe.UltimaActualizacion,
            Observaciones = informe.Observaciones
        };
    }

    private InformePublicoDTO MapearInformePublico(InformeExterno informe)
    {
        return new InformePublicoDTO
        {
            Estado = informe.Estado,
            Score = informe.Score,
            Riesgo = informe.Riesgo,
            UltimaActualizacion = informe.UltimaActualizacion,
            Observaciones = informe.Observaciones
        };
    }
} 