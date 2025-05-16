using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Shared.DTOs.Cliente;

namespace PortalAnalisisCrediticio.Core.Interfaces;

/// <summary>
/// Servicio para la gestión de clientes
/// </summary>
public interface IClienteService
{
    /// <summary>
    /// Obtiene todos los clientes con paginación y filtros
    /// </summary>
    /// <param name="page">Número de página</param>
    /// <param name="size">Tamaño de página</param>
    /// <param name="filtros">Filtros opcionales</param>
    /// <returns>Lista paginada de clientes</returns>
    Task<IEnumerable<ClienteDTO>> GetAllAsync(int page = 1, int size = 10, Dictionary<string, string> filtros = null);

    /// <summary>
    /// Obtiene un cliente por su ID
    /// </summary>
    /// <param name="id">ID del cliente</param>
    /// <returns>Datos del cliente</returns>
    Task<ClienteDTO> GetByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo cliente
    /// </summary>
    /// <param name="cliente">Datos del cliente a crear</param>
    /// <returns>Cliente creado</returns>
    Task<ClienteDTO> CreateAsync(CreateClienteDTO cliente);

    /// <summary>
    /// Actualiza los datos de un cliente
    /// </summary>
    /// <param name="id">ID del cliente</param>
    /// <param name="cliente">Datos actualizados</param>
    /// <returns>Cliente actualizado</returns>
    Task<ClienteDTO> UpdateAsync(int id, UpdateClienteDTO cliente);

    /// <summary>
    /// Elimina un cliente
    /// </summary>
    /// <param name="id">ID del cliente</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Obtiene el historial de cambios de un cliente
    /// </summary>
    /// <param name="id">ID del cliente</param>
    /// <returns>Lista de cambios realizados</returns>
    Task<IEnumerable<HistorialClienteDTO>> GetHistorialAsync(int id);

    /// <summary>
    /// Busca clientes por diferentes criterios
    /// </summary>
    /// <param name="criterio">Criterio de búsqueda</param>
    /// <returns>Lista de clientes que coinciden con el criterio</returns>
    Task<IEnumerable<ClienteDTO>> SearchAsync(string criterio);

    Task<IEnumerable<ClienteDTO>> GetAllWithDetailsAsync();
    Task<ClienteDTO> GetByIdWithDetailsAsync(int id);
    Task<ClienteDTO> CreateWithDetailsAsync(ClienteDTO clienteDto);
    Task<ClienteDTO> UpdateWithDetailsAsync(int id, ClienteDTO clienteDto);
    Task<IEnumerable<ClienteDTO>> SearchAsync(string searchTerm);
    Task<ClienteDTO> GetByEmailAsync(string email);
    Task<ClienteDTO> GetByDocumentoAsync(string documento);
    Task<ClienteDTO> ImportarDesdeExcelAsync(Stream fileStream);
} 