using Microsoft.EntityFrameworkCore;
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

    public InformeService(
        ApplicationDbContext context,
        IAnalisisRiesgoService analisisRiesgoService,
        IConverter converter)
    {
        _context = context;
        _analisisRiesgoService = analisisRiesgoService;
        _converter = converter;
    }

    public async Task<InformeDTO> GenerarInformeAsync(int clienteId)
    {
        // Obtener datos del cliente
        var cliente = await _context.Clientes
            .Include(c => c.SolicitudesProducto)
            .FirstOrDefaultAsync(c => c.Id == clienteId);

        if (cliente == null)
            throw new Exception("Cliente no encontrado");

        // Obtener análisis de riesgo
        var analisisRiesgo = await _analisisRiesgoService.GetInformeRiesgoAsync(clienteId);

        // Obtener historial de informes
        var historialInformes = await GetHistorialInformesAsync(clienteId);

        // Generar recomendaciones
        var recomendaciones = GenerarRecomendaciones(analisisRiesgo, cliente.SolicitudesProducto);

        return new InformeDTO
        {
            ClienteId = clienteId,
            FechaGeneracion = DateTime.Now,
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
    }

    public async Task<byte[]> ExportarInformePDFAsync(int clienteId)
    {
        var informe = await GenerarInformeAsync(clienteId);

        // Crear el HTML del informe
        var html = $@"
            <html>
            <head>
                <meta charset='utf-8'>
                <style>
                    body {{ font-family: Arial, sans-serif; margin: 40px; }}
                    .header {{ text-align: center; margin-bottom: 30px; }}
                    .section {{ margin-bottom: 20px; }}
                    .section-title {{ color: #2c3e50; border-bottom: 2px solid #eee; padding-bottom: 10px; }}
                    .score {{ color: #2c3e50; font-weight: bold; }}
                    .riesgo-bajo {{ color: #27ae60; }}
                    .riesgo-medio {{ color: #f39c12; }}
                    .riesgo-alto {{ color: #c0392b; }}
                    table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
                    th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
                    th {{ background-color: #f5f5f5; }}
                    .historial-item {{ margin-bottom: 15px; padding: 10px; background-color: #f9f9f9; }}
                </style>
            </head>
            <body>
                <div class='header'>
                    <h1>Informe de Análisis Crediticio</h1>
                    <p>Fecha: {informe.FechaGeneracion:dd/MM/yyyy}</p>
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
                    <p>Score Total: <span class='score'>{informe.AnalisisRiesgo.ScoreTotal:P2}</span></p>
                    <p><strong>Justificación:</strong> {informe.AnalisisRiesgo.Justificacion}</p>
                    <p><strong>Recomendación:</strong> {(informe.AnalisisRiesgo.RecomendacionOtorgarCredito ? "Aprobado" : "No Aprobado")}</p>
                    <p><strong>Tasa de Interés Sugerida:</strong> {informe.AnalisisRiesgo.TasaInteresSugerida:P2}</p>
                    <p><strong>Garantías Adicionales:</strong> {informe.AnalisisRiesgo.GarantiasAdicionalesSugeridas}</p>
                    <p><strong>Plazo Máximo:</strong> {informe.AnalisisRiesgo.PlazoMaximoSugerido} meses</p>
                </div>

                <div class='section'>
                    <h2 class='section-title'>Recomendaciones</h2>
                    <p>{informe.Recomendaciones}</p>
                </div>

                <div class='section'>
                    <h2 class='section-title'>Historial de Informes</h2>
                    {string.Join("", informe.HistorialInformes.Select(h => $@"
                    <div class='historial-item'>
                        <p><strong>Fecha:</strong> {h.FechaGeneracion:dd/MM/yyyy}</p>
                        <p><strong>Nivel de Riesgo:</strong> <span class='riesgo-{h.NivelRiesgo.ToLower()}'>{h.NivelRiesgo}</span></p>
                        <p><strong>Recomendación:</strong> {(h.RecomendacionOtorgarCredito ? "Aprobado" : "No Aprobado")}</p>
                        <p><strong>Tasa de Interés:</strong> {h.TasaInteresSugerida:P2}</p>
                        <p><strong>Garantías:</strong> {h.GarantiasAdicionalesSugeridas}</p>
                    </div>"))}
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

    public async Task<IEnumerable<InformeHistoricoDTO>> GetHistorialInformesAsync(int clienteId)
    {
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

        return informes;
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
        }
        else if (analisisRiesgo.NivelRiesgo == "Medio")
        {
            recomendaciones.Add("Se recomienda otorgar el crédito con las garantías sugeridas.");
            recomendaciones.Add("Considerar un seguimiento más frecuente del cliente.");
            recomendaciones.Add("Evaluar la posibilidad de reducir el monto total solicitado.");
        }
        else
        {
            recomendaciones.Add("Se recomienda otorgar el crédito en las condiciones solicitadas.");
            recomendaciones.Add("Mantener un seguimiento regular del cliente.");
        }

        // Recomendaciones basadas en los productos solicitados
        var montoTotal = solicitudes.Sum(s => s.MontoTotal);
        if (montoTotal > 1000000)
        {
            recomendaciones.Add("Considerar la posibilidad de fraccionar la compra en múltiples operaciones.");
        }

        return string.Join("\n", recomendaciones);
    }
} 