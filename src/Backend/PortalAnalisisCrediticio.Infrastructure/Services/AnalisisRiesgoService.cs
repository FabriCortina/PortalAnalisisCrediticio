using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.AnalisisRiesgo;
using PortalAnalisisCrediticio.Shared.DTOs.Integraciones;
using DinkToPdf;
using DinkToPdf.Contracts;
using PortalAnalisisCrediticio.Core.Domain.Entities;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class AnalisisRiesgoService : IAnalisisRiesgoService
{
    private readonly ApplicationDbContext _context;
    private readonly IIntegracionesExternasService _integracionesService;
    private readonly IConverter _converter;
    private readonly ILogger<AnalisisRiesgoService> _logger;
    private readonly IMemoryCache _cache;
    private const double PESO_HISTORIAL_PAGOS = 0.3;
    private const double PESO_SITUACION_FINANCIERA = 0.3;
    private const double PESO_INFORMES_EXTERNOS = 0.25;
    private const double PESO_GARANTIAS = 0.15;
    private const int CACHE_DURATION_MINUTES = 60;

    public AnalisisRiesgoService(
        ApplicationDbContext context,
        IIntegracionesExternasService integracionesService,
        IConverter converter,
        ILogger<AnalisisRiesgoService> logger,
        IMemoryCache cache)
    {
        _context = context;
        _integracionesService = integracionesService;
        _converter = converter;
        _logger = logger;
        _cache = cache;
    }

    public async Task<InformeRiesgoDTO> RealizarAnalisisRiesgoAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Iniciando análisis de riesgo para el cliente {clienteId}");

            // Verificar caché
            var cacheKey = $"analisis_riesgo_{clienteId}";
            if (_cache.TryGetValue(cacheKey, out InformeRiesgoDTO informeCache))
            {
                _logger.LogInformation($"Retornando análisis de riesgo desde caché para el cliente {clienteId}");
                return informeCache;
            }

            // Obtener información del cliente con todas las relaciones necesarias
            var cliente = await _context.Clientes
                .Include(c => c.InformacionFinanciera)
                .Include(c => c.Deudas)
                .Include(c => c.Garantias)
                .FirstOrDefaultAsync(c => c.Id == clienteId);

            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {clienteId} no encontrado");
                throw new Exception($"Cliente {clienteId} no encontrado");
            }

            // Validar información financiera
            if (cliente.InformacionFinanciera == null)
            {
                _logger.LogWarning($"Información financiera no disponible para el cliente {clienteId}");
                throw new Exception("Información financiera no disponible");
            }

            // Obtener informes externos en paralelo con timeout
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var tareasInformes = new[]
            {
                _integracionesService.GetInformeNosisAsync(clienteId),
                _integracionesService.GetInformeVerazAsync(clienteId),
                _integracionesService.GetInformeBCRAAsync(clienteId),
                _integracionesService.GetInformeAFIPAsync(clienteId)
            };

            var informes = await Task.WhenAll(tareasInformes);
            var informeNosis = informes[0];
            var informeVeraz = informes[1];
            var informeBCRA = informes[2];
            var informeAFIP = informes[3];

            // Calcular scores individuales con validaciones
            var scoreHistorialPagos = CalcularScoreHistorialPagos(cliente.Deudas);
            var scoreSituacionFinanciera = CalcularScoreSituacionFinanciera(cliente.InformacionFinanciera);
            var scoreInformesExternos = CalcularScoreInformesExternos(informeNosis, informeVeraz, informeBCRA, informeAFIP);
            var scoreGarantias = CalcularScoreGarantias(cliente);

            // Calcular score total ponderado
            var scoreTotal = (scoreHistorialPagos * PESO_HISTORIAL_PAGOS) +
                            (scoreSituacionFinanciera * PESO_SITUACION_FINANCIERA) +
                            (scoreInformesExternos * PESO_INFORMES_EXTERNOS) +
                            (scoreGarantias * PESO_GARANTIAS);

            var nivelRiesgo = DeterminarNivelRiesgo(scoreTotal);

            // Crear informe
            var informe = new InformeRiesgoDTO
            {
                ClienteId = clienteId,
                NivelRiesgo = nivelRiesgo,
                ScoreHistorialPagos = scoreHistorialPagos,
                ScoreSituacionFinanciera = scoreSituacionFinanciera,
                ScoreInformesExternos = scoreInformesExternos,
                ScoreGarantias = scoreGarantias,
                ScoreTotal = scoreTotal,
                Justificacion = GenerarJustificacion(nivelRiesgo, scoreTotal),
                RecomendacionOtorgarCredito = DeterminarRecomendacion(nivelRiesgo),
                TasaInteresSugerida = CalcularTasaInteres(nivelRiesgo),
                GarantiasAdicionalesSugeridas = DeterminarGarantiasAdicionales(nivelRiesgo),
                PlazoMaximoSugerido = DeterminarPlazoMaximo(nivelRiesgo),
                FechaAnalisis = DateTime.UtcNow
            };

            // Guardar informe en la base de datos
            var informeEntity = new InformeRiesgo
            {
                ClienteId = clienteId,
                NivelRiesgo = informe.NivelRiesgo,
                Justificacion = informe.Justificacion,
                RecomendacionOtorgarCredito = informe.RecomendacionOtorgarCredito,
                TasaInteresSugerida = informe.TasaInteresSugerida,
                GarantiasAdicionalesSugeridas = informe.GarantiasAdicionalesSugeridas,
                PlazoMaximoSugerido = informe.PlazoMaximoSugerido,
                ScoreHistorialPagos = informe.ScoreHistorialPagos,
                ScoreSituacionFinanciera = informe.ScoreSituacionFinanciera,
                ScoreInformesExternos = informe.ScoreInformesExternos,
                ScoreGarantias = informe.ScoreGarantias,
                ScoreTotal = informe.ScoreTotal,
                FechaAnalisis = informe.FechaAnalisis
            };

            _context.InformesRiesgo.Add(informeEntity);
            await _context.SaveChangesAsync();

            // Guardar en caché
            _cache.Set(cacheKey, informe, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Análisis de riesgo completado para el cliente {clienteId}. Nivel de riesgo: {nivelRiesgo}");
            return informe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al realizar análisis de riesgo para el cliente {clienteId}");
            throw new Exception($"Error al realizar análisis de riesgo: {ex.Message}", ex);
        }
    }

    public async Task<InformeRiesgoDTO> GetInformeRiesgoAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Obteniendo informe de riesgo para el cliente {clienteId}");

            // Verificar caché
            var cacheKey = $"informe_riesgo_{clienteId}";
            if (_cache.TryGetValue(cacheKey, out InformeRiesgoDTO informeCache))
            {
                _logger.LogInformation($"Retornando informe de riesgo desde caché para el cliente {clienteId}");
                return informeCache;
            }

            var informe = await _context.InformesRiesgo
                .Where(i => i.ClienteId == clienteId)
                .OrderByDescending(i => i.FechaAnalisis)
                .FirstOrDefaultAsync();

            if (informe == null)
            {
                _logger.LogWarning($"No se encontró informe de riesgo para el cliente {clienteId}");
                throw new Exception($"No se encontró informe de riesgo para el cliente {clienteId}");
            }

            var informeDTO = new InformeRiesgoDTO
            {
                Id = informe.Id,
                ClienteId = informe.ClienteId,
                NivelRiesgo = informe.NivelRiesgo,
                Justificacion = informe.Justificacion,
                RecomendacionOtorgarCredito = informe.RecomendacionOtorgarCredito,
                TasaInteresSugerida = informe.TasaInteresSugerida,
                GarantiasAdicionalesSugeridas = informe.GarantiasAdicionalesSugeridas,
                PlazoMaximoSugerido = informe.PlazoMaximoSugerido,
                ScoreHistorialPagos = informe.ScoreHistorialPagos,
                ScoreSituacionFinanciera = informe.ScoreSituacionFinanciera,
                ScoreInformesExternos = informe.ScoreInformesExternos,
                ScoreGarantias = informe.ScoreGarantias,
                ScoreTotal = informe.ScoreTotal,
                FechaAnalisis = informe.FechaAnalisis
            };

            // Guardar en caché
            _cache.Set(cacheKey, informeDTO, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Informe de riesgo obtenido exitosamente para el cliente {clienteId}");
            return informeDTO;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener informe de riesgo para el cliente {clienteId}");
            throw new Exception($"Error al obtener informe de riesgo: {ex.Message}", ex);
        }
    }

    public async Task<byte[]> ExportarInformePDFAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Iniciando exportación de informe PDF para el cliente {clienteId}");

            var informe = await GetInformeRiesgoAsync(clienteId);
            if (informe == null)
            {
                throw new Exception($"No se encontró informe de riesgo para el cliente {clienteId}");
            }

            var cliente = await _context.Clientes
                .Include(c => c.InformacionFinanciera)
                .FirstOrDefaultAsync(c => c.Id == clienteId);

            if (cliente == null)
            {
                throw new Exception($"Cliente {clienteId} no encontrado");
            }

            var html = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; margin: 40px; }}
                        .header {{ text-align: center; margin-bottom: 30px; }}
                        .section {{ margin-bottom: 20px; }}
                        .section-title {{ color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 5px; }}
                        table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
                        th, td {{ padding: 10px; border: 1px solid #ddd; text-align: left; }}
                        th {{ background-color: #f5f5f5; }}
                        .riesgo-bajo {{ color: green; }}
                        .riesgo-medio {{ color: orange; }}
                        .riesgo-alto {{ color: red; }}
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <h1>Informe de Análisis de Riesgo</h1>
                        <p>Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                    </div>

                    <div class='section'>
                        <h2 class='section-title'>Información del Cliente</h2>
                        <p><strong>Nombre:</strong> {cliente.Nombre} {cliente.Apellido}</p>
                        <p><strong>Documento:</strong> {cliente.Documento}</p>
                        <p><strong>Tipo de Cliente:</strong> {cliente.TipoCliente}</p>
                    </div>

                    <div class='section'>
                        <h2 class='section-title'>Análisis de Riesgo</h2>
                        <p>Nivel de Riesgo: <span class='riesgo-{informe.NivelRiesgo.ToLower()}'>{informe.NivelRiesgo}</span></p>
                        <p>Score Total: {informe.ScoreTotal:P2}</p>
                        <p><strong>Justificación:</strong> {informe.Justificacion}</p>
                        <p><strong>Recomendación:</strong> {(informe.RecomendacionOtorgarCredito ? "Aprobado" : "No Aprobado")}</p>
                        <p><strong>Tasa de Interés Sugerida:</strong> {informe.TasaInteresSugerida:P2}</p>
                        <p><strong>Garantías Adicionales:</strong> {informe.GarantiasAdicionalesSugeridas}</p>
                        <p><strong>Plazo Máximo:</strong> {informe.PlazoMaximoSugerido} meses</p>
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
                                <td>{informe.ScoreHistorialPagos:P2}</td>
                            </tr>
                            <tr>
                                <td>Situación Financiera</td>
                                <td>{informe.ScoreSituacionFinanciera:P2}</td>
                            </tr>
                            <tr>
                                <td>Informes Externos</td>
                                <td>{informe.ScoreInformesExternos:P2}</td>
                            </tr>
                            <tr>
                                <td>Garantías</td>
                                <td>{informe.ScoreGarantias:P2}</td>
                            </tr>
                        </table>
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

    private double CalcularScoreHistorialPagos(ICollection<Deuda> deudas)
    {
        if (deudas == null || !deudas.Any())
            return 0.5;

        var score = 1.0;
        var totalDeudas = deudas.Count;
        var deudasIncumplidas = deudas.Count(d => d.Estado == "Incumplimiento");
        var deudasAtrasadas = deudas.Count(d => d.Estado == "Atraso");

        // Penalización por incumplimientos
        score -= (deudasIncumplidas * 0.3) / totalDeudas;
        
        // Penalización por atrasos
        score -= (deudasAtrasadas * 0.15) / totalDeudas;

        return Math.Max(0, Math.Min(1, score));
    }

    private double CalcularScoreSituacionFinanciera(InformacionFinanciera info)
    {
        if (info == null)
            return 0.5;

        var score = 0.5;

        // Análisis de ingresos vs deudas
        if (info.IngresosMensuales > 0 && info.DeudasMensuales > 0)
        {
            var ratio = info.IngresosMensuales / info.DeudasMensuales;
            if (ratio >= 3) score += 0.3;
            else if (ratio >= 2) score += 0.2;
            else if (ratio >= 1) score += 0.1;
        }

        // Análisis de patrimonio
        if (info.PatrimonioNeto > 0)
        {
            if (info.PatrimonioNeto > 1000000) score += 0.2;
            else if (info.PatrimonioNeto > 500000) score += 0.1;
        }

        // Análisis de antigüedad
        if (info.AntiguedadLaboral > 0)
        {
            if (info.AntiguedadLaboral >= 5) score += 0.1;
            else if (info.AntiguedadLaboral >= 2) score += 0.05;
        }

        return Math.Max(0, Math.Min(1, score));
    }

    private double CalcularScoreInformesExternos(
        InformeNosisDTO nosis,
        InformeVerazDTO veraz,
        InformeBCRADTO bcra,
        InformeAFIPDTO afip)
    {
        var score = 0.5;
        var informesValidos = 0;

        // Análisis de Nosis
        if (nosis != null)
        {
            informesValidos++;
            switch (nosis.Score)
            {
                case "A": score += 0.1; break;
                case "B": score += 0.05; break;
                case "C": score -= 0.05; break;
                case "D": score -= 0.1; break;
            }
        }

        // Análisis de Veraz
        if (veraz != null)
        {
            informesValidos++;
            switch (veraz.Score)
            {
                case "A": score += 0.1; break;
                case "B": score += 0.05; break;
                case "C": score -= 0.05; break;
                case "D": score -= 0.1; break;
            }
        }

        // Análisis de BCRA
        if (bcra != null)
        {
            informesValidos++;
            switch (bcra.Score)
            {
                case "A": score += 0.1; break;
                case "B": score += 0.05; break;
                case "C": score -= 0.05; break;
                case "D": score -= 0.1; break;
            }
        }

        // Análisis de AFIP
        if (afip != null)
        {
            informesValidos++;
            switch (afip.Score)
            {
                case "A": score += 0.1; break;
                case "B": score += 0.05; break;
                case "C": score -= 0.05; break;
                case "D": score -= 0.1; break;
            }
        }

        // Normalizar el score basado en la cantidad de informes válidos
        if (informesValidos > 0)
            score = score / informesValidos;

        return Math.Max(0, Math.Min(1, score));
    }

    private double CalcularScoreGarantias(Cliente cliente)
    {
        if (cliente == null || cliente.Garantias == null || !cliente.Garantias.Any())
            return 0.0;

        var score = 0.0;
        var garantiasValidas = cliente.Garantias.Where(g => g.Estado == "Registrada").ToList();

        if (!garantiasValidas.Any())
            return 0.0;

        foreach (var garantia in garantiasValidas)
        {
            switch (garantia.Tipo)
            {
                case "Real":
                    if (garantia.ValorEstimado > 0)
                    {
                        var ratio = garantia.ValorEstimado / garantia.MontoGarantizado;
                        if (ratio >= 2.0) score += 0.3;
                        else if (ratio >= 1.5) score += 0.2;
                        else if (ratio >= 1.0) score += 0.1;
                    }
                    break;

                case "Personal":
                    if (garantia.Avalista != null)
                    {
                        var avalistaScore = CalcularScoreHistorialPagos(garantia.Avalista.Deudas);
                        score += avalistaScore * 0.2;
                    }
                    break;

                case "Prendaria":
                    if (garantia.ValorEstimado > 0)
                    {
                        var ratio = garantia.ValorEstimado / garantia.MontoGarantizado;
                        if (ratio >= 1.5) score += 0.2;
                        else if (ratio >= 1.2) score += 0.15;
                        else if (ratio >= 1.0) score += 0.1;
                    }
                    break;

                case "Fiduciaria":
                    score += 0.15;
                    break;
            }
        }

        // Normalizar el score final
        return Math.Min(1.0, score / garantiasValidas.Count);
    }

    private string DeterminarNivelRiesgo(double scoreTotal)
    {
        if (scoreTotal >= 0.8) return "Bajo";
        if (scoreTotal >= 0.5) return "Medio";
        return "Alto";
    }

    private string GenerarJustificacion(string nivelRiesgo, double scoreTotal)
    {
        var justificacion = $"Nivel de riesgo: {nivelRiesgo}. ";

        if (nivelRiesgo == "Bajo")
            justificacion += "El cliente presenta un perfil crediticio sólido con buen historial de pagos y situación financiera estable.";
        else if (nivelRiesgo == "Medio")
            justificacion += "El cliente presenta un perfil crediticio aceptable con algunas áreas de mejora en su historial crediticio.";
        else
            justificacion += "El cliente presenta un perfil crediticio de alto riesgo con incidencias significativas en su historial.";

        return justificacion;
    }

    private bool DeterminarRecomendacion(string nivelRiesgo)
    {
        return nivelRiesgo != "Alto";
    }

    private decimal CalcularTasaInteres(string nivelRiesgo)
    {
        return nivelRiesgo switch
        {
            "Bajo" => 20.0m,
            "Medio" => 30.0m,
            "Alto" => 40.0m,
            _ => 30.0m
        };
    }

    private string DeterminarGarantiasAdicionales(string nivelRiesgo)
    {
        return nivelRiesgo switch
        {
            "Bajo" => "No se requieren garantías adicionales",
            "Medio" => "Se sugiere garantía prendaria",
            "Alto" => "Se requieren garantías reales y personales",
            _ => "Evaluar caso a caso"
        };
    }

    private int DeterminarPlazoMaximo(string nivelRiesgo)
    {
        return nivelRiesgo switch
        {
            "Bajo" => 36,
            "Medio" => 24,
            "Alto" => 12,
            _ => 24
        };
    }

    public async Task<AnalisisRiesgoResponseDTO> AnalizarRiesgoAsync(AnalisisRiesgoRequestDTO request)
    {
        try
        {
            // TODO: En el futuro, aquí se integrará con un modelo de ML entrenado
            var razones = new List<string>();
            var nivelRiesgo = CalcularNivelRiesgo(request, razones);
            var recomendacion = DeterminarRecomendacion(nivelRiesgo, request);
            var condiciones = CalcularCondicionesSugeridas(nivelRiesgo, request);

            return new AnalisisRiesgoResponseDTO
            {
                NivelRiesgo = nivelRiesgo,
                Razones = razones,
                Recomendacion = recomendacion,
                CondicionesSugeridas = condiciones,
                FechaAnalisis = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al analizar el riesgo crediticio");
            throw;
        }
    }

    private string CalcularNivelRiesgo(AnalisisRiesgoRequestDTO request, List<string> razones)
    {
        var score = 0;

        // Análisis de capacidad de pago
        if (request.InformacionFinanciera != null)
        {
            var ingresosMensuales = request.InformacionFinanciera.IngresosMensuales;
            var deudaTotal = request.DeudasActuales?.Sum(d => d.MontoMensual) ?? 0;
            var capacidadPago = ingresosMensuales - deudaTotal;
            var ratioDeuda = deudaTotal / ingresosMensuales;

            if (ratioDeuda > 0.7m)
            {
                score += 3;
                razones.Add("Alto nivel de endeudamiento actual");
            }
            else if (ratioDeuda > 0.5m)
            {
                score += 2;
                razones.Add("Nivel de endeudamiento moderado");
            }
            else
            {
                score += 1;
                razones.Add("Bajo nivel de endeudamiento");
            }
        }

        // Análisis de garantías
        var valorGarantias = request.Garantias?.Sum(g => g.ValorEstimado) ?? 0;
        var ratioGarantias = valorGarantias / request.MontoSolicitado;

        if (ratioGarantias < 1.2m)
        {
            score += 3;
            razones.Add("Garantías insuficientes para el monto solicitado");
        }
        else if (ratioGarantias < 1.5m)
        {
            score += 2;
            razones.Add("Garantías moderadas para el monto solicitado");
        }
        else
        {
            score += 1;
            razones.Add("Garantías sólidas para el monto solicitado");
        }

        // Análisis de plazo
        if (request.PlazoMeses > 60)
        {
            score += 2;
            razones.Add("Plazo de financiamiento extenso");
        }
        else if (request.PlazoMeses > 36)
        {
            score += 1;
            razones.Add("Plazo de financiamiento moderado");
        }

        // Clasificación final
        if (score >= 6)
            return "Alto";
        else if (score >= 3)
            return "Medio";
        else
            return "Bajo";
    }

    private bool DeterminarRecomendacion(string nivelRiesgo, AnalisisRiesgoRequestDTO request)
    {
        return nivelRiesgo switch
        {
            "Bajo" => true,
            "Medio" => request.Garantias?.Any() == true,
            "Alto" => false,
            _ => false
        };
    }

    private CondicionesSugeridasDTO CalcularCondicionesSugeridas(string nivelRiesgo, AnalisisRiesgoRequestDTO request)
    {
        var condiciones = new CondicionesSugeridasDTO
        {
            GarantiasAdicionales = new List<string>(),
            CondicionesEspeciales = new List<string>()
        };

        switch (nivelRiesgo)
        {
            case "Bajo":
                condiciones.TasaInteresSugerida = 0.15m; // 15% anual
                condiciones.MontoMaximoSugerido = request.MontoSolicitado;
                condiciones.PlazoMaximoSugerido = request.PlazoMeses;
                break;

            case "Medio":
                condiciones.TasaInteresSugerida = 0.25m; // 25% anual
                condiciones.MontoMaximoSugerido = request.MontoSolicitado * 0.8m;
                condiciones.PlazoMaximoSugerido = Math.Min(request.PlazoMeses, 36);
                condiciones.GarantiasAdicionales.Add("Garantía hipotecaria o prendaria");
                condiciones.CondicionesEspeciales.Add("Revisión trimestral de estados financieros");
                break;

            case "Alto":
                condiciones.TasaInteresSugerida = 0.35m; // 35% anual
                condiciones.MontoMaximoSugerido = request.MontoSolicitado * 0.5m;
                condiciones.PlazoMaximoSugerido = Math.Min(request.PlazoMeses, 24);
                condiciones.GarantiasAdicionales.Add("Garantía hipotecaria");
                condiciones.GarantiasAdicionales.Add("Aval solidario");
                condiciones.CondicionesEspeciales.Add("Revisión mensual de estados financieros");
                condiciones.CondicionesEspeciales.Add("Pago anticipado de intereses");
                break;
        }

        return condiciones;
    }
} 