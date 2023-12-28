using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using TrabajoFinalRestaurante.Domain.DTO;
using TrabajoFinalRestaurante.Domain.Entities;
using TrabajoFinalRestaurante.Entities;

namespace TrabajoFinalRestaurante.Repository
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ReservaRestaurantContext _restaurantContext;

        public ReservaRepository(ReservaRestaurantContext context)
        {
            _restaurantContext = context;
        }
        public async Task<bool> AddReservaRepositoryAsync(ReservaDTO Reserva)
        {
            var NuevaReserva = new Reserva();
            NuevaReserva.CodReserva = Guid.NewGuid().ToString();
            NuevaReserva.NombrePersona = Reserva.NombrePersona.ToUpper();
            NuevaReserva.ApellidoPersona = Reserva.ApellidoPersona.ToUpper(); ;
            NuevaReserva.Dni = Reserva.Dni;
            NuevaReserva.Mail = Reserva.Mail;
            NuevaReserva.Celular = Reserva.Celular;
            NuevaReserva.FechaReserva = Reserva.FechaReserva;
            NuevaReserva.IdRangoReserva = Reserva.IdRangoReserva;
            NuevaReserva.CantidadPersonas = Reserva.CantidadPersonas;
            NuevaReserva.Estado = "CONFIRMADO";
            NuevaReserva.FechaAlta = DateTime.Now;
            NuevaReserva.FechaModificacion = null;
            await _restaurantContext.Reservas.AddAsync(NuevaReserva);
            await _restaurantContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateReservaRepositoryAsync(ModificacionReservaDTO ReservaModif) //nuevo 
        {
            var reserva = await _restaurantContext.Reservas.Where(w => w.Dni == ReservaModif.Dni && w.Estado == "CONFIRMADO").FirstOrDefaultAsync();
            reserva.FechaReserva = ReservaModif.NuevaFechaReserva;
            reserva.IdRangoReserva = ReservaModif.NuevoIdRangoReserva;
            reserva.CantidadPersonas = ReservaModif.CantidadPersonas;
            reserva.FechaModificacion = DateTime.Now;
            await _restaurantContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CancelarReservaRepositoryAsync(string Dni) // nuevo
        {
            var Reserva = await _restaurantContext.Reservas.Where(w => w.Dni == Dni && w.Estado == "CONFIRMADO").FirstOrDefaultAsync();
            if (Reserva != null)
            {
                Reserva.Estado = "CANCELADO";
                Reserva.FechaModificacion = DateTime.Now;
                await _restaurantContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }


        public async Task<List<Reserva>> TurnosCanceladosRepository() //nuevo
        {
            return await _restaurantContext.Reservas.Where(w => w.Estado == "CANCELADO").ToListAsync();

        }

        public async Task<List<Reserva>> TurnosConfirmadosRepository() //nuevo
        {
            return await _restaurantContext.Reservas.Where(w => w.Estado == "CONFIRMADO").ToListAsync();
        }

        public Task<List<Reserva>> TurnosLibresPorFechaRepository(DateTime Fecha)
        {
            throw new NotImplementedException();
        }
        //    public async Task<List<>> TurnosLibresPorFechaRepository(DateTime Fecha)
        //    {
        //        var result = await _restaurantContext.Reservas.Where(x => x.FechaReserva.Date == Fecha.Date)
        //.Select(x => new 
        //{
        //    IdRangoReserva = x.IdRangoReserva,
        //    DescripcionRango = x.IdRangoReservaNavigation.Descripcion,
        //    CupoTotal = x.IdRangoReservaNavigation.Cupo,
        //    CupoOcupado = _restaurantContext.Reservas
        //        .Where(r => r.FechaReserva.Date == Fecha.Date && r.IdRangoReserva == x.IdRangoReserva)
        //        .Sum(r => r.CantidadPersonas),
        //    LugaresLibres = x.IdRangoReservaNavigation.Cupo - _restaurantContext.Reservas
        //        .Where(r => r.FechaReserva.Date == Fecha.Date && r.IdRangoReserva == x.IdRangoReserva)
        //        .Sum(r => r.CantidadPersonas)
        //}).ToListAsync();
        //        //.Reservas.Where(w => w.FechaReserva == Fecha && w.Estado == "CONFIRMADO")
        //        //                                  .GroupBy(x => x.IdRangoReserva)
        //        //                                  .Select(group => new
        //        //                                  {
        //        //                                      IdRangoReserva = group.Key,
        //        //                                      CuposOcupados = group.Sum(x => x.CantidadPersonas),
        //        //                                      Rango = group.Select(s => s.IdRangoReservaNavigation.Descripcion).FirstOrDefault()
        //        //                                  }).ToListAsync();
        //        //return result.Select(s => new Reserva
        //        //{
        //        //    IdRangoReserva = s.IdRangoReserva,
        //        //    CantidadPersonas = s.CuposOcupados,
        //        //    IdRangoReservaNavigation = new RangoReserva
        //        //    {
        //        //        Descripcion = s.Rango
        //        //    }
        //        //}).ToList();

        //        return result;


        //    }


    }
}
