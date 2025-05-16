using System.Collections.Generic;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

/// <summary>
/// DTO que contiene la información del dashboard para administradores
/// </summary>
public class DashboardAdminDTO
{
    /// <summary>
    /// KPIs específicos para administradores
    /// </summary>
    public KPIsAdminDTO KPIs { get; set; }

    /// <summary>
    /// Métricas de rendimiento del sistema
    /// </summary>
    public MetricasSistemaDTO MetricasSistema { get; set; }

    /// <summary>
    /// Actividad de usuarios
    /// </summary>
    public List<ActividadUsuarioDTO> ActividadUsuarios { get; set; }

    /// <summary>
    /// Alertas del sistema
    /// </summary>
    public List<AlertaSistemaDTO> AlertasSistema { get; set; }

    /// <summary>
    /// Estadísticas de uso
    /// </summary>
    public EstadisticasUsoDTO EstadisticasUso { get; set; }
}

/// <summary>
/// DTO que contiene los KPIs específicos para administradores
/// </summary>
public class KPIsAdminDTO
{
    /// <summary>
    /// Total de usuarios activos
    /// </summary>
    public int TotalUsuarios { get; set; }

    /// <summary>
    /// Total de sesiones activas
    /// </summary>
    public int SesionesActivas { get; set; }

    /// <summary>
    /// Tiempo promedio de respuesta del sistema
    /// </summary>
    public decimal TiempoRespuestaPromedio { get; set; }

    /// <summary>
    /// Tasa de errores del sistema
    /// </summary>
    public decimal TasaErrores { get; set; }
}

/// <summary>
/// DTO que contiene las métricas del sistema
/// </summary>
public class MetricasSistemaDTO
{
    /// <summary>
    /// Uso de CPU
    /// </summary>
    public decimal UsoCPU { get; set; }

    /// <summary>
    /// Uso de memoria
    /// </summary>
    public decimal UsoMemoria { get; set; }

    /// <summary>
    /// Espacio en disco utilizado
    /// </summary>
    public decimal EspacioDisco { get; set; }

    /// <summary>
    /// Tiempo de actividad del sistema
    /// </summary>
    public TimeSpan TiempoActividad { get; set; }
}

/// <summary>
/// DTO que representa la actividad de un usuario
/// </summary>
public class ActividadUsuarioDTO
{
    /// <summary>
    /// Nombre de usuario
    /// </summary>
    public string Usuario { get; set; }

    /// <summary>
    /// Última actividad realizada
    /// </summary>
    public string UltimaActividad { get; set; }

    /// <summary>
    /// Fecha y hora de la última actividad
    /// </summary>
    public DateTime FechaHora { get; set; }

    /// <summary>
    /// IP desde donde se conectó
    /// </summary>
    public string IP { get; set; }
}

/// <summary>
/// DTO que representa una alerta del sistema
/// </summary>
public class AlertaSistemaDTO
{
    /// <summary>
    /// Tipo de alerta
    /// </summary>
    public string Tipo { get; set; }

    /// <summary>
    /// Mensaje de la alerta
    /// </summary>
    public string Mensaje { get; set; }

    /// <summary>
    /// Nivel de severidad
    /// </summary>
    public string Severidad { get; set; }

    /// <summary>
    /// Fecha y hora de la alerta
    /// </summary>
    public DateTime FechaHora { get; set; }
}

/// <summary>
/// DTO que contiene estadísticas de uso del sistema
/// </summary>
public class EstadisticasUsoDTO
{
    /// <summary>
    /// Peticiones por hora
    /// </summary>
    public Dictionary<string, int> PeticionesPorHora { get; set; }

    /// <summary>
    /// Peticiones por endpoint
    /// </summary>
    public Dictionary<string, int> PeticionesPorEndpoint { get; set; }

    /// <summary>
    /// Tiempo promedio de respuesta por endpoint
    /// </summary>
    public Dictionary<string, decimal> TiempoRespuestaPorEndpoint { get; set; }

    /// <summary>
    /// Errores por tipo
    /// </summary>
    public Dictionary<string, int> ErroresPorTipo { get; set; }
} 