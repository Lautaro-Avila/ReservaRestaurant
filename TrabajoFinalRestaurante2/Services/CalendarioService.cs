using System.Globalization;
using TrabajoFinalRestaurante.Domain.DTO;
using TrabajoFinalRestaurante.Domain.Entities;
using TrabajoFinalRestaurante.Repository;
using TrabajoFinalRestaurante.Services.Interfaces;

namespace TrabajoFinalRestaurante.Services
{
    public class CalendarioService : ICalendarioService
    {

        private readonly IReservaRepository _reservaRepository;
        private readonly ReservaRestaurantContext _restaurantContext;

        public CalendarioService(IReservaRepository reservaRepository, ReservaRestaurantContext restaurantContext)
        {
            _reservaRepository = reservaRepository;
            _restaurantContext = restaurantContext;

        }

        public Task<Calendario> GetCalendarioAsync()
        {
            throw new NotImplementedException();
        }
        //public async Task<Calendario> GetCalendarioAsync()
        //{
        //    var calendario = new Calendario ();
        //    var Fecha = DateTime.Now;
        //    var FechaInicio = DateTime.Now;

        //   while ((Fecha -FechaInicio).TotalDays <= 7)
        //    {
        //        var fechaActual = FechaInicio.AddDays(i);
        //        var DiaSemana = fechaActual.DayOfWeek;

        //        var rangos = await _restaurantContext.RangoReservas.Select(rango => new RangoInfo
        //        {
        //            Rango = rango.Descripcion,
        //            Reservas = new Reservas
        //            {
        //                Ocupados = _restaurantContext.Reservas
        //                                    .Where(reserva =>
        //                                        reserva.FechaReserva.Date == Fecha.Date &&
        //                                        reserva.IdRangoReserva == rango.IdRangoReserva &&
        //                                        reserva.Estado == "CONFIRMADO")
        //                                    .Sum(reserva => reserva.CantidadPersonas),

        //                Libres = rango.Cupo - reserva.Ocupados

        //                Total = rango.Cupo
        //            }
        //        }).ToListAsync();

        //        calendario.Add(new CalendarioDTO
        //        {
        //            Fecha = Fecha,
        //            Dia = diaSemana,
        //            Rangos = rangos
        //        });
        //    }

        //    return calendario;
        //}


    }
    }
