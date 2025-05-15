using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.AnalisisRiesgo;
using PortalAnalisisCrediticio.Shared.DTOs.Integraciones;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class AnalisisRiesgoService : IAnalisisRiesgoService
{
    private readonly ApplicationDbContext _context;
    private readonly IIntegracionesExternasService _integracionesService;
    private readonly IConverter _converter;

    public AnalisisRiesgoService(
        ApplicationDbContext context,
        IIntegracionesExternasService integracionesService,
        IConverter converter)
    {
        _context = context;
        _integracionesService = integracionesService;
        _converter = converter;
    }

    public async Task<InformeRiesgoDTO> RealizarAnalisisRiesgoAsync(int clienteId)
    {
        // Obtener información del cliente
        var cliente = await _context.Clientes
            .Include(c => c.InformacionFinanciera)
            .Include(c => c.Deudas)
            .FirstOrDefaultAsync(c => c.Id == clienteId);

        if (cliente == null)
            throw new Exception("Cliente no encontrado");

        // Obtener informes externos
        var informeNosis = await _integracionesService.GetInformeNosisAsync(clienteId);
        var informeVeraz = await _integracionesService.GetInformeVerazAsync(clienteId);
        var informeBCRA = await _integracionesService.GetInformeBCRAAsync(clienteId);
        var informeAFIP = await _integracionesService.GetInformeAFIPAsync(clienteId);

        // Calcular scores individuales
        var scoreHistorialPagos = CalcularScoreHistorialPagos(cliente.Deudas);
        var scoreSituacionFinanciera = CalcularScoreSituacionFinanciera(cliente.InformacionFinanciera);
        var scoreInformesExternos = CalcularScoreInformesExternos(informeNosis, informeVeraz, informeBCRA, informeAFIP);
        var scoreGarantias = CalcularScoreGarantias(cliente);

        // Calcular score total y nivel de riesgo
        var scoreTotal = (scoreHistorialPagos + scoreSituacionFinanciera + scoreInformesExternos + scoreGarantias) / 4;
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
            FechaAnalisis = DateTime.Now
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

        return informe;
    }

    public async Task<InformeRiesgoDTO> GetInformeRiesgoAsync(int clienteId)
    {
        var informe = await _context.InformesRiesgo
            .Where(i => i.ClienteId == clienteId)
            .OrderByDescending(i => i.FechaAnalisis)
            .FirstOrDefaultAsync();

        if (informe == null)
            throw new Exception("No se encontró un informe de riesgo para el cliente");

        return new InformeRiesgoDTO
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
    }

    public async Task<byte[]> ExportarInformePDFAsync(int clienteId)
    {
        var informe = await GetInformeRiesgoAsync(clienteId);
        var cliente = await _context.Clientes
            .Include(c => c.InformacionFinanciera)
            .FirstOrDefaultAsync(c => c.Id == clienteId);

        if (cliente == null)
            throw new Exception("Cliente no encontrado");

        // Crear el HTML del informe
        var html = $@"
            <html>
            <head>
                <meta charset='utf-8'>
                <style>
                    body {{ font-family: Arial, sans-serif; margin: 40px; }}
                    .header {{ text-align: center; margin-bottom: 30px; }}
                    .section {{ margin-bottom: 20px; }}
                    .score {{ color: #2c3e50; font-weight: bold; }}
                    .riesgo-bajo {{ color: #27ae60; }}
                    .riesgo-medio {{ color: #f39c12; }}
                    .riesgo-alto {{ color: #c0392b; }}
                    table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
                    th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
                    th {{ background-color: #f5f5f5; }}
                </style>
            </head>
            <body>
                <div class='header'>
                    <h1>Informe de Análisis de Riesgo Crediticio</h1>
                    <p>Fecha: {informe.FechaAnalisis:dd/MM/yyyy}</p>
                </div>

                <div class='section'>
                    <h2>Información del Cliente</h2>
                    <p>Nombre: {cliente.Nombre} {cliente.Apellido}</p>
                    <p>CUIT: {cliente.CUIT}</p>
                </div>

                <div class='section'>
                    <h2>Resultado del Análisis</h2>
                    <p>Nivel de Riesgo: <span class='riesgo-{informe.NivelRiesgo.ToLower()}'>{informe.NivelRiesgo}</span></p>
                    <p>Score Total: <span class='score'>{informe.ScoreTotal:P2}</span></p>
                </div>

                <div class='section'>
                    <h2>Desglose de Scores</h2>
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

                <div class='section'>
                    <h2>Recomendaciones</h2>
                    <p><strong>Justificación:</strong> {informe.Justificacion}</p>
                    <p><strong>Recomendación de Crédito:</strong> {(informe.RecomendacionOtorgarCredito ? "Aprobado" : "No Aprobado")}</p>
                    <p><strong>Tasa de Interés Sugerida:</strong> {informe.TasaInteresSugerida:P2}</p>
                    <p><strong>Garantías Adicionales:</strong> {informe.GarantiasAdicionalesSugeridas}</p>
                    <p><strong>Plazo Máximo Sugerido:</strong> {informe.PlazoMaximoSugerido} meses</p>
                </div>
            </body>
            </html>";

        // Configurar las opciones de conversión a PDF
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

        // Convertir HTML a PDF
        return _converter.Convert(doc);
    }

    private double CalcularScoreHistorialPagos(ICollection<Deuda> deudas)
    {
        if (deudas == null || !deudas.Any())
            return 0.5; // Score neutral si no hay deudas

        var score = 1.0;
        foreach (var deuda in deudas)
        {
            if (deuda.Estado == "Incumplimiento")
                score -= 0.2;
            else if (deuda.Estado == "Atraso")
                score -= 0.1;
        }

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

        return Math.Max(0, Math.Min(1, score));
    }

    private double CalcularScoreInformesExternos(
        InformeNosisDTO nosis,
        InformeVerazDTO veraz,
        InformeBCRADTO bcra,
        InformeAFIPDTO afip)
    {
        var score = 0.5;

        // Análisis de Nosis
        if (nosis.Score == "A") score += 0.1;
        else if (nosis.Score == "B") score += 0.05;
        else if (nosis.Score == "C") score -= 0.05;
        else if (nosis.Score == "D") score -= 0.1;

        // Análisis de Veraz
        if (veraz.Score == "A") score += 0.1;
        else if (veraz.Score == "B") score += 0.05;
        else if (veraz.Score == "C") score -= 0.05;
        else if (veraz.Score == "D") score -= 0.1;

        // Análisis de BCRA
        if (bcra.Score == "A") score += 0.1;
        else if (bcra.Score == "B") score += 0.05;
        else if (bcra.Score == "C") score -= 0.05;
        else if (bcra.Score == "D") score -= 0.1;

        // Análisis de AFIP
        if (afip.Score == "A") score += 0.1;
        else if (afip.Score == "B") score += 0.05;
        else if (afip.Score == "C") score -= 0.05;
        else if (afip.Score == "D") score -= 0.1;

        return Math.Max(0, Math.Min(1, score));
    }

    private double CalcularScoreGarantias(Cliente cliente)
    {
        if (cliente == null)
            return 0.0;

        var score = 0.0;
        var garantias = _context.Garantias
            .Where(g => g.ClienteId == cliente.Id)
            .ToList();

        if (!garantias.Any())
            return 0.0;

        foreach (var garantia in garantias)
        {
            switch (garantia.Tipo)
            {
                case "Real":
                    // Garantías reales (inmuebles, vehículos, etc.)
                    if (garantia.ValorEstimado > 0)
                    {
                        var ratio = garantia.ValorEstimado / garantia.MontoGarantizado;
                        if (ratio >= 2.0) score += 0.3;
                        else if (ratio >= 1.5) score += 0.2;
                        else if (ratio >= 1.0) score += 0.1;
                    }
                    break;

                case "Personal":
                    // Garantías personales (aval, fianza, etc.)
                    if (garantia.Avalista != null)
                    {
                        // Verificar historial crediticio del avalista
                        var avalistaScore = CalcularScoreHistorialPagos(garantia.Avalista.Deudas);
                        score += avalistaScore * 0.2;
                    }
                    break;

                case "Prendaria":
                    // Garantías prendarias (bienes muebles)
                    if (garantia.ValorEstimado > 0)
                    {
                        var ratio = garantia.ValorEstimado / garantia.MontoGarantizado;
                        if (ratio >= 1.5) score += 0.2;
                        else if (ratio >= 1.2) score += 0.15;
                        else if (ratio >= 1.0) score += 0.1;
                    }
                    break;

                case "Fiduciaria":
                    // Garantías fiduciarias
                    score += 0.15;
                    break;
            }

            // Bonificación por garantías registradas
            if (garantia.Estado == "Registrada")
                score += 0.1;
        }

        // Normalizar el score final
        return Math.Min(1.0, score);
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
} 