using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Services;
using PortalAnalisisCrediticio.Infrastructure.Mappings;
using System.Text.Json.Serialization;
using DinkToPdf;
using DinkToPdf.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Services
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IInformacionFinancieraService, InformacionFinancieraService>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IAnalisisRiesgoService, AnalisisRiesgoService>();
builder.Services.AddScoped<IIntegracionesExternasService, IntegracionesExternasService>();

// Registrar DinkToPdf
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
