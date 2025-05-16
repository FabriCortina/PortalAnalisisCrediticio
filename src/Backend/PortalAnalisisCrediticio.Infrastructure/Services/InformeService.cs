using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.Informes;
using PortalAnalisisCrediticio.Shared.DTOs.AnalisisRiesgo;
using PortalAnalisisCrediticio.Shared.DTOs.Producto;
using PortalAnalisisCrediticio.Shared.DTOs.Cliente;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class InformeService : IInformeService
{
    private readonly ApplicationDbContext _context;
    private readonly IAnalisisRiesgoService _analisisRiesgoService;
    private readonly IConverter _converter;
    private readonly ILogger<InformeService> _logger;
    private readonly IMemoryCache _cache;
    private const int CACHE_DURATION_MINUTES = 60;

    public InformeService(
        ApplicationDbContext context,
        IAnalisisRiesgoService analisisRiesgoService,
        IConverter converter,
        ILogger<InformeService> logger,
        IMemoryCache cache)
    {
        _context = context;
        _analisisRiesgoService = analisisRiesgoService;
        _converter = converter;
        _logger = logger;
        _cache = cache;
    }

    public async Task<InformeDTO> GenerarInformeAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Iniciando generación de informe para el cliente {clienteId}");

            // Verificar caché
            var cacheKey = $"informe_{clienteId}";
            if (_cache.TryGetValue(cacheKey, out InformeDTO informeCache))
            {
                _logger.LogInformation($"Retornando informe desde caché para el cliente {clienteId}");
                return informeCache;
            }

            // Obtener datos del cliente con todas las relaciones necesarias
            var cliente = await _context.Clientes
                .Include(c => c.SolicitudesProducto)
                    .ThenInclude(sp => sp.Producto)
                .Include(c => c.InformacionFinanciera)
                .Include(c => c.Deudas)
                .Include(c => c.Garantias)
                .FirstOrDefaultAsync(c => c.Id == clienteId);

            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {clienteId} no encontrado");
                throw new Exception($"Cliente {clienteId} no encontrado");
            }

            // Obtener análisis de riesgo
            var analisisRiesgo = await _analisisRiesgoService.GetInformeRiesgoAsync(clienteId);

            // Obtener historial de informes
            var historialInformes = await GetHistorialInformesAsync(clienteId);

            // Generar recomendaciones
            var recomendaciones = GenerarRecomendaciones(analisisRiesgo, cliente.SolicitudesProducto);

            var informe = new InformeDTO
            {
                ClienteId = clienteId,
                FechaGeneracion = DateTime.UtcNow,
                Cliente = new ClienteDTO
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    CUIT = cliente.CUIT,
                    Email = cliente.Email,
                    Telefono = cliente.Telefono
                },
                ProductosSolicitados = cliente.SolicitudesProducto.Select(sp => new SolicitudProductoDTO
                {
                    Id = sp.Id,
                    Producto = sp.Producto,
                    Cantidad = sp.Cantidad,
                    MontoTotal = sp.MontoTotal,
                    Estado = sp.Estado
                }).ToList(),
                AnalisisRiesgo = analisisRiesgo,
                Recomendaciones = recomendaciones,
                HistorialInformes = historialInformes.ToList()
            };

            // Guardar en caché
            _cache.Set(cacheKey, informe, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Informe generado exitosamente para el cliente {clienteId}");
            return informe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al generar informe para el cliente {clienteId}");
            throw new Exception($"Error al generar informe: {ex.Message}", ex);
        }
    }

    public async Task<byte[]> ExportarInformePDFAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Iniciando exportación de informe PDF para el cliente {clienteId}");

            var informe = await GenerarInformeAsync(clienteId);
            if (informe == null)
            {
                throw new Exception($"No se encontró informe para el cliente {clienteId}");
            }

            // Crear el HTML del informe con mejor formato y estilos
            var html = $@"
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; margin: 40px; }}
                        .header {{ text-align: center; margin-bottom: 30px; }}
                        .section {{ margin-bottom: 20px; }}
                        .section-title {{ color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 5px; }}
                        .score {{ color: #2c3e50; font-weight: bold; }}
                        .riesgo-bajo {{ color: #27ae60; }}
                        .riesgo-medio {{ color: #f39c12; }}
                        .riesgo-alto {{ color: #c0392b; }}
                        table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
                        th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
                        th {{ background-color: #f5f5f5; }}
                        .historial-item {{ margin-bottom: 15px; padding: 10px; background-color: #f9f9f9; }}
                        .recomendaciones {{ background-color: #f8f9fa; padding: 15px; border-left: 4px solid #3498db; }}
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <h1>Informe de Análisis Crediticio</h1>
                        <p>Fecha: {informe.FechaGeneracion:dd/MM/yyyy HH:mm}</p>
                    </div>

                    <div class='section'>
                        <h2 class='section-title'>Información del Cliente</h2>
                        <p><strong>Nombre:</strong> {informe.Cliente.Nombre} {informe.Cliente.Apellido}</p>
                        <p><strong>CUIT:</strong> {informe.Cliente.CUIT}</p>
                        <p><strong>Email:</strong> {informe.Cliente.Email}</p>
                        <p><strong>Teléfono:</strong> {informe.Cliente.Telefono}</p>
                    </div>

                    <div class='section'>
                        <h2 class='section-title'>Productos Solicitados</h2>
                        <table>
                            <tr>
                                <th>Producto</th>
                                <th>Cantidad</th>
                                <th>Monto Total</th>
                                <th>Estado</th>
                            </tr>
                            {string.Join("", informe.ProductosSolicitados.Select(p => $@"
                                <tr>
                                    <td>{p.Producto}</td>
                                    <td>{p.Cantidad}</td>
                                    <td>${p.MontoTotal:N2}</td>
                                    <td>{p.Estado}</td>
                                </tr>"))}
                        </table>
                    </div>

                    <div class='section'>
                        <h2 class='section-title'>Análisis de Riesgo</h2>
                        <p>Nivel de Riesgo: <span class='riesgo-{informe.AnalisisRiesgo.NivelRiesgo.ToLower()}'>{informe.AnalisisRiesgo.NivelRiesgo}</span></p>
                        <p>Score Total: {informe.AnalisisRiesgo.ScoreTotal:P2}</p>
                        <p><strong>Justificación:</strong> {informe.AnalisisRiesgo.Justificacion}</p>
                        <p><strong>Recomendación:</strong> {(informe.AnalisisRiesgo.RecomendacionOtorgarCredito ? "Aprobado" : "No Aprobado")}</p>
                        <p><strong>Tasa de Interés Sugerida:</strong> {informe.AnalisisRiesgo.TasaInteresSugerida:P2}</p>
                        <p><strong>Garantías Adicionales:</strong> {informe.AnalisisRiesgo.GarantiasAdicionalesSugeridas}</p>
                        <p><strong>Plazo Máximo:</strong> {informe.AnalisisRiesgo.PlazoMaximoSugerido} meses</p>
                    </div>

                    <div class='section'>
                        <h2 class='section-title'>Scores Detallados</h2>
                        <table>
                            <tr>
                                <th>Factor</th>
                                <th>Score</th>
                            </tr>
                            <tr>
                                <td>Historial de Pagos</td>
                                <td>{informe.AnalisisRiesgo.ScoreHistorialPagos:P2}</td>
                            </tr>
                            <tr>
                                <td>Situación Financiera</td>
                                <td>{informe.AnalisisRiesgo.ScoreSituacionFinanciera:P2}</td>
                            </tr>
                            <tr>
                                <td>Informes Externos</td>
                                <td>{informe.AnalisisRiesgo.ScoreInformesExternos:P2}</td>
                            </tr>
                            <tr>
                                <td>Garantías</td>
                                <td>{informe.AnalisisRiesgo.ScoreGarantias:P2}</td>
                            </tr>
                        </table>
                    </div>

                    <div class='section'>
                        <h2 class='section-title'>Recomendaciones</h2>
                        <div class='recomendaciones'>
                            {informe.Recomendaciones}
                        </div>
                    </div>

                    <div class='section'>
                        <h2 class='section-title'>Historial de Informes</h2>
                        {string.Join("", informe.HistorialInformes.Select(h => $@"
                            <div class='historial-item'>
                                <p><strong>Fecha:</strong> {h.FechaGeneracion:dd/MM/yyyy}</p>
                                <p><strong>Nivel de Riesgo:</strong> {h.NivelRiesgo}</p>
                                <p><strong>Recomendación:</strong> {(h.RecomendacionOtorgarCredito ? "Aprobado" : "No Aprobado")}</p>
                                <p><strong>Tasa Sugerida:</strong> {h.TasaInteresSugerida:P2}</p>
                                <p><strong>Garantías:</strong> {h.GarantiasAdicionalesSugeridas}</p>
                            </div>"))}
                    </div>
                </body>
                </html>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 },
                },
                Objects = {
                    new ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Página [page] de [toPage]", Line = true },
                        UseLocalLinks = true,
                        ProduceForms = true,
                    }
                }
            };

            var pdfBytes = _converter.Convert(doc);
            _logger.LogInformation($"Informe PDF generado exitosamente para el cliente {clienteId}");
            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al exportar informe PDF para el cliente {clienteId}");
            throw new Exception($"Error al exportar informe PDF: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<InformeHistoricoDTO>> GetHistorialInformesAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Obteniendo historial de informes para el cliente {clienteId}");

            // Verificar caché
            var cacheKey = $"historial_informes_{clienteId}";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<InformeHistoricoDTO> historialCache))
            {
                _logger.LogInformation($"Retornando historial de informes desde caché para el cliente {clienteId}");
                return historialCache;
            }

            var informes = await _context.InformesRiesgo
                .Where(i => i.ClienteId == clienteId)
                .OrderByDescending(i => i.FechaAnalisis)
                .Select(i => new InformeHistoricoDTO
                {
                    Id = i.Id,
                    FechaGeneracion = i.FechaAnalisis,
                    NivelRiesgo = i.NivelRiesgo,
                    RecomendacionOtorgarCredito = i.RecomendacionOtorgarCredito,
                    TasaInteresSugerida = i.TasaInteresSugerida,
                    GarantiasAdicionalesSugeridas = i.GarantiasAdicionalesSugeridas
                })
                .ToListAsync();

            // Guardar en caché
            _cache.Set(cacheKey, informes, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Historial de informes obtenido exitosamente para el cliente {clienteId}");
            return informes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener historial de informes para el cliente {clienteId}");
            throw new Exception($"Error al obtener historial de informes: {ex.Message}", ex);
        }
    }

    private string GenerarRecomendaciones(InformeRiesgoDTO analisisRiesgo, ICollection<SolicitudProducto> solicitudes)
    {
        var recomendaciones = new List<string>();

        // Recomendaciones basadas en el nivel de riesgo
        if (analisisRiesgo.NivelRiesgo == "Alto")
        {
            recomendaciones.Add("Se recomienda no otorgar el crédito en las condiciones actuales.");
            recomendaciones.Add("Solicitar garantías adicionales si se considera otorgar el crédito.");
            recomendaciones.Add("Considerar reducir el monto solicitado o aumentar el plazo de pago.");
            recomendaciones.Add("Realizar un seguimiento más frecuente del cliente.");
        }
        else if (analisisRiesgo.NivelRiesgo == "Medio")
        {
            recomendaciones.Add("Se recomienda otorgar el crédito con las garantías sugeridas.");
            recomendaciones.Add("Considerar un seguimiento más frecuente del cliente.");
            recomendaciones.Add("Evaluar la posibilidad de reducir el monto total solicitado.");
            recomendaciones.Add("Establecer condiciones más estrictas para el otorgamiento.");
        }
        else
        {
            recomendaciones.Add("Se recomienda otorgar el crédito en las condiciones solicitadas.");
            recomendaciones.Add("Mantener un seguimiento regular del cliente.");
            recomendaciones.Add("Considerar ofrecer productos adicionales.");
        }

        // Recomendaciones basadas en las solicitudes
        var montoTotal = solicitudes.Sum(s => s.MontoTotal);
        if (montoTotal > 1000000)
        {
            recomendaciones.Add("Considerar fraccionar el monto total en múltiples operaciones.");
        }

        return string.Join("\n", recomendaciones);
    }
} 