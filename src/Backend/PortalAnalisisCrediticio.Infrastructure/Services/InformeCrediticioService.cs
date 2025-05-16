using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.InformeCrediticio;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class InformeCrediticioService : IInformeCrediticioService
{
    private readonly ApplicationDbContext _context;
    private readonly IConverter _converter;
    private readonly ILogger<InformeCrediticioService> _logger;

    public InformeCrediticioService(
        ApplicationDbContext context,
        IConverter converter,
        ILogger<InformeCrediticioService> logger)
    {
        _context = context;
        _converter = converter;
        _logger = logger;
    }

    public async Task<InformeCrediticioDTO> GetByIdAsync(int id)
    {
        var informe = await _context.InformesCrediticio
            .Include(i => i.Cliente)
            .Include(i => i.SolicitudProducto)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (informe == null)
            return null;

        var informeDto = MapToDTO(informe);
        informeDto.InformesAnteriores = await GetInformesAnterioresAsync(informe.ClienteId, id);
        return informeDto;
    }

    public async Task<IEnumerable<InformeCrediticioDTO>> GetByClienteIdAsync(int clienteId)
    {
        var informes = await _context.InformesCrediticio
            .Include(i => i.Cliente)
            .Include(i => i.SolicitudProducto)
            .Where(i => i.ClienteId == clienteId && i.Activo)
            .OrderByDescending(i => i.FechaAnalisis)
            .ToListAsync();

        return informes.Select(MapToDTO);
    }

    public async Task<InformeCrediticioDTO> CreateAsync(CreateInformeCrediticioDTO informeDto)
    {
        var informe = new InformeCrediticio
        {
            ClienteId = informeDto.ClienteId,
            SolicitudProductoId = informeDto.SolicitudProductoId,
            NivelRiesgo = informeDto.NivelRiesgo,
            Justificacion = informeDto.Justificacion,
            RecomendacionOtorgarCredito = informeDto.RecomendacionOtorgarCredito,
            TasaInteresSugerida = informeDto.TasaInteresSugerida,
            GarantiasAdicionalesSugeridas = informeDto.GarantiasAdicionalesSugeridas,
            PlazoMaximoSugerido = informeDto.PlazoMaximoSugerido,
            ScoreHistorialPagos = informeDto.ScoreHistorialPagos,
            ScoreSituacionFinanciera = informeDto.ScoreSituacionFinanciera,
            ScoreInformesExternos = informeDto.ScoreInformesExternos,
            ScoreGarantias = informeDto.ScoreGarantias,
            ScoreTotal = informeDto.ScoreTotal,
            Observaciones = informeDto.Observaciones,
            Autor = informeDto.Autor,
            FechaAnalisis = DateTime.UtcNow,
            Activo = true
        };

        _context.InformesCrediticio.Add(informe);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(informe.Id);
    }

    public async Task<InformeCrediticioDTO> UpdateAsync(int id, UpdateInformeCrediticioDTO informeDto)
    {
        var informe = await _context.InformesCrediticio.FindAsync(id);
        if (informe == null)
            return null;

        informe.NivelRiesgo = informeDto.NivelRiesgo;
        informe.Justificacion = informeDto.Justificacion;
        informe.RecomendacionOtorgarCredito = informeDto.RecomendacionOtorgarCredito;
        informe.TasaInteresSugerida = informeDto.TasaInteresSugerida;
        informe.GarantiasAdicionalesSugeridas = informeDto.GarantiasAdicionalesSugeridas;
        informe.PlazoMaximoSugerido = informeDto.PlazoMaximoSugerido;
        informe.Observaciones = informeDto.Observaciones;
        informe.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var informe = await _context.InformesCrediticio.FindAsync(id);
        if (informe != null)
        {
            informe.Activo = false;
            informe.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<byte[]> ExportarPDFAsync(int id)
    {
        var informe = await GetByIdAsync(id);
        if (informe == null)
            throw new Exception("Informe no encontrado");

        var html = await GenerarVistaWebAsync(id);

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

        return _converter.Convert(doc);
    }

    public async Task<string> GenerarVistaWebAsync(int id)
    {
        var informe = await GetByIdAsync(id);
        if (informe == null)
            throw new Exception("Informe no encontrado");

        return $@"
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
                    <h1>Informe Crediticio</h1>
                    <p>Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                </div>

                <div class='section'>
                    <h2 class='section-title'>Información del Cliente</h2>
                    <p><strong>Nombre:</strong> {informe.Cliente.Nombre} {informe.Cliente.Apellido}</p>
                    <p><strong>Documento:</strong> {informe.Cliente.Documento}</p>
                    <p><strong>Tipo de Cliente:</strong> {informe.Cliente.TipoCliente}</p>
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

                <div class='section'>
                    <h2 class='section-title'>Histórico de Informes</h2>
                    <table>
                        <tr>
                            <th>Fecha</th>
                            <th>Nivel Riesgo</th>
                            <th>Score Total</th>
                            <th>Recomendación</th>
                        </tr>
                        {string.Join("", informe.InformesAnteriores.Select(i => $@"
                            <tr>
                                <td>{i.FechaAnalisis:dd/MM/yyyy}</td>
                                <td class='riesgo-{i.NivelRiesgo.ToLower()}'>{i.NivelRiesgo}</td>
                                <td>{i.ScoreTotal:P2}</td>
                                <td>{(i.RecomendacionOtorgarCredito ? "Aprobado" : "No Aprobado")}</td>
                            </tr>
                        "))}
                    </table>
                </div>
            </body>
            </html>";
    }

    private async Task<List<InformeCrediticioDTO>> GetInformesAnterioresAsync(int clienteId, int informeActualId)
    {
        return await _context.InformesCrediticio
            .Where(i => i.ClienteId == clienteId && i.Id != informeActualId && i.Activo)
            .OrderByDescending(i => i.FechaAnalisis)
            .Take(5)
            .Select(i => MapToDTO(i))
            .ToListAsync();
    }

    private InformeCrediticioDTO MapToDTO(InformeCrediticio informe)
    {
        return new InformeCrediticioDTO
        {
            Id = informe.Id,
            ClienteId = informe.ClienteId,
            SolicitudProductoId = informe.SolicitudProductoId,
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
            FechaAnalisis = informe.FechaAnalisis,
            FechaActualizacion = informe.FechaActualizacion,
            Observaciones = informe.Observaciones,
            Autor = informe.Autor,
            Activo = informe.Activo,
            Cliente = informe.Cliente != null ? new ClienteDTO
            {
                Id = informe.Cliente.Id,
                Nombre = informe.Cliente.Nombre,
                Apellido = informe.Cliente.Apellido,
                Documento = informe.Cliente.Documento,
                TipoCliente = informe.Cliente.TipoCliente
            } : null,
            SolicitudProducto = informe.SolicitudProducto != null ? new SolicitudProductoDTO
            {
                Id = informe.SolicitudProducto.Id,
                MontoSolicitado = informe.SolicitudProducto.MontoSolicitado,
                PlazoMeses = informe.SolicitudProducto.PlazoMeses
            } : null
        };
    }
} 