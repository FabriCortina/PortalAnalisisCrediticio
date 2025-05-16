using PortalAnalisisCrediticio.Shared.DTOs.Alerta;

namespace PortalAnalisisCrediticio.Core.Interfaces
{
    public interface IAlertaService
    {
        Task<IEnumerable<AlertaDTO>> GetAllAsync();
        Task<AlertaDTO> GetByIdAsync(int id);
        Task<IEnumerable<AlertaDTO>> GetByClienteIdAsync(int clienteId);
        Task<AlertaDTO> CreateAsync(CreateAlertaDTO dto);
        Task<AlertaDTO> UpdateAsync(int id, UpdateAlertaDTO dto);
        Task DeleteAsync(int id);
        Task ProcesarAlertasAsync(); // Lógica para procesar y enviar notificaciones
    }
} 