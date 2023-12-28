using System.Globalization;
using TrabajoFinalRestaurante.Domain.DTO;

namespace TrabajoFinalRestaurante.Services.Interfaces
{
    public interface ICalendarioService
    {
        public  Task<Calendario> GetCalendarioAsync();
       
    }
}
