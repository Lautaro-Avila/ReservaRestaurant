using TrabajoFinalRestaurante.Controllers;
using TrabajoFinalRestaurante.Domain.DTO;
using TrabajoFinalRestaurante.Domain.Entities;

namespace TrabajoFinalRestaurante.Services.Interfaces
{
    public interface IReservaService
    {
        public Task<ReservaResponse> AddReservaAsync(ReservaDTO Reserva);
        public Task<ReservaResponse> UpdateReservaAsync( ModificacionReservaDTO ReservaModif);
        public Task<bool> CancelarReservaAsync(string Dni); 

        public Task<List<Reserva>> TurnosCanceladosAsync(); 

        public Task<List<Reserva>> TurnosConfirmadosAsync();

        public Task<List<Reserva>> TurnosLibresPorFechaAsync(DateTime Fecha);
    }
}
