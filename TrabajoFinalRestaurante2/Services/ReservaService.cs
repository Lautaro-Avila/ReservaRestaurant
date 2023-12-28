using Microsoft.EntityFrameworkCore;
using TrabajoFinalRestaurante.Controllers;
using TrabajoFinalRestaurante.Domain.DTO;
using TrabajoFinalRestaurante.Domain.Entities;
using TrabajoFinalRestaurante.Repository;
using TrabajoFinalRestaurante.Services.Interfaces;

namespace TrabajoFinalRestaurante.Services
{
    public class ReservaService : IReservaService

    {

        private readonly IReservaRepository _reservaRepository;
        private readonly ReservaRestaurantContext _restaurantContext;

        public ReservaService(IReservaRepository reservaRepository, ReservaRestaurantContext restaurantContext)
        {
            _reservaRepository = reservaRepository;
            _restaurantContext=restaurantContext;

        }
        public async Task<ReservaResponse> AddReservaAsync(ReservaDTO Reserva)
        {
            var response =  new ReservaResponse();

             if (!ReservaUnica(Reserva.FechaReserva, Reserva.Dni))
            {
                response.Success = false;
                response.Message = "El DNI ingresado ya posee una reserva ese dia, para continuar, ingrese una fecha nueva o cancele la reserva actual.";
                return response;
            }

            if (!ValidarFecha(Reserva.FechaReserva))
                            {
                            response.Success = false;
                            response.Message = "La reserva debe ser con, maximo, 7 dias de anticipación.";
                            return response;
                        }

            if (!RangoValido(Reserva.IdRangoReserva))
            {
                response.Success = false;
                response.Message = "El rango horario solicitado no existe.";
                return response;
            }

            if (!CupoValido( Reserva.IdRangoReserva, Reserva.CantidadPersonas, Reserva.FechaReserva))
                {
                    response.Success = false;
                    response.Message = "El turno seleccionado no posee cupos suficientes.";
                    return response;
                }
        
            var result = await _reservaRepository.AddReservaRepositoryAsync(Reserva);
            response.Success = true;
            response.Message = "Reserva realizada correctamente.";  
            return response; 
            //
        }
        public async Task<ReservaResponse> UpdateReservaAsync(ModificacionReservaDTO ReservaModif)
        {
            var response = new ReservaResponse();


            var reserva = await _restaurantContext.Reservas.Where(w => w.Dni == ReservaModif.Dni && w.Estado == "CONFIRMADO").FirstOrDefaultAsync();
            if (reserva != null)
            {
                response.Success = false;
                response.Message = "La reserva no existe o ha sido Cancelada.";
                return response;
            }

            if (!ValidarFecha(ReservaModif.NuevaFechaReserva))

            {
                response.Success = false;
                response.Message = "La reserva debe ser con, maximo, 7 dias de anticipación.";
                return response;
            }

            if (!RangoValido(ReservaModif.NuevoIdRangoReserva))
            {
                response.Success = false;
                response.Message = "El rango horario solicitado no existe.";
                return response;
            }
            if (!CupoValido(ReservaModif.NuevoIdRangoReserva, ReservaModif.CantidadPersonas, ReservaModif.NuevaFechaReserva))
            {
                response.Success = false;
                response.Message = "El turno seleccionado no posee cupos suficientes.";
                return response;
            }
           
            if (!ReservaUnica(ReservaModif.NuevaFechaReserva, ReservaModif.Dni)) //ReservaUnicaUpdate
            {
                response.Success = false;
                response.Message = "El DNI ingresado ya posee una reserva ese dia, para continuar, ingrese una fecha nueva o cancele la reserva actual.";
                return response;
            }
            var result = await _reservaRepository.UpdateReservaRepositoryAsync (ReservaModif);
            response.Success = true;
            response.Message = "Reserva modificada correctamente.";
            return response;
        }
       
        private bool RangoValido (int IdRango)
        {
            var result = _restaurantContext.RangoReservas.Where(w => w.IdRangoReserva == IdRango).FirstOrDefaultAsync();
            if (result == null) { return false; }
            else
            { return true; }
            
        }
        public async Task<bool> CancelarReservaAsync(string Dni) //nuevo 
        {
            var result = await _reservaRepository.CancelarReservaRepositoryAsync(Dni);
            return result;
        }

        public async Task<List<Reserva>> TurnosCanceladosAsync() //nuevo
        {
            var result = await _reservaRepository.TurnosCanceladosRepository();
            return result;
        }

        public async Task<List<Reserva>> TurnosConfirmadosAsync() //nuevo
        {
            var result = await _reservaRepository.TurnosConfirmadosRepository();
            return result;
        }
        public async Task<List<Reserva>> TurnosLibresPorFechaAsync(DateTime fecha)
        {
            var result = await _reservaRepository.TurnosLibresPorFechaRepository(fecha);
            return result;
        }

        private bool ValidarFecha(DateTime FechaReserva)
        {
            if ((FechaReserva - DateTime.Now).TotalDays > 7)
            {
                return false;
            }
            return true;
        }
        private bool ReservaUnica(DateTime FReserva, string DniReserva)
        {
            var result = _restaurantContext.Reservas.Count(w => w.FechaReserva == FReserva && w.Dni == DniReserva && w.Estado == "CONFIRMADO");
            //var result =_restaurantContext.Reservas.Where(w => w.FechaReserva == FReserva && w.Estado=="CONFIRMADO")
            //                                        .Select(s => s.Dni == DniReserva).Count();
            if (result >0)
            {
                return false;

            }
            return true;
        }
        //private bool ReservaUnicaUpdate(DateTime FReserva, string DniReserva,string CodReserva)
        //{
        //    var result = _restaurantContext.Reservas.Where(w => w.FechaReserva == FReserva && w.Estado == "CONFIRMADO")
        //                                            .Select(s => s.Dni == DniReserva).Count();

        //}
        private int CuposDisponibles(int idRango, DateTime Fecha)
        {
                var Ocupados = _restaurantContext.Reservas
         .Where(x => x.FechaReserva == Fecha && x.IdRangoReserva == idRango && x.Estado == "CONFIRMADO")
         .GroupBy(x => x.IdRangoReserva)
         .Select(group => new
         {
             IdRangoReserva = group.Key,
             CuposOcupados = group.Sum(x => x.CantidadPersonas)
         })
         .FirstOrDefault();

            var Cupos = _restaurantContext.RangoReservas.Where(w => w.IdRangoReserva == idRango)
                                                       .Select(s => s.Cupo).FirstOrDefault();
           
            if (Ocupados == null)
            {
                var Disponibles = Cupos;
                return Disponibles;
            }
            else
            { var Disponibles = Cupos - Ocupados.CuposOcupados;
                return Disponibles;
            }
           
            
        }
        private bool CupoValido (int idRango,int CantidadPersonas, DateTime FechaReserva)
        {
          if(  CuposDisponibles(idRango,FechaReserva) - CantidadPersonas < 0)
            {
                return false;
            }
            return true;
        }

       
    }
}
