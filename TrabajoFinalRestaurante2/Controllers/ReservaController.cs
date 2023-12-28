using Microsoft.AspNetCore.Mvc;
using TrabajoFinalRestaurante.Domain.DTO;
using TrabajoFinalRestaurante.Domain.Entities;
using TrabajoFinalRestaurante.Services.Interfaces;

namespace TrabajoFinalRestaurante.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class ReservaController : ControllerBase
    {

        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpPost("AddReserva")]
        public async Task<IActionResult> AddReserva([FromBody] ReservaDTO NuevaReserva)
        {
            var result = await _reservaService.AddReservaAsync(NuevaReserva);
            if (!result.Success) return BadRequest(result.Message);

            return Created("", result.Message);
        }
        [HttpPut("UpdateReserva/{dni}")]
        public async Task<IActionResult> UpdateReserva([FromBody] ModificacionReservaDTO ReservaModif)
        {
            var result = await _reservaService.UpdateReservaAsync(ReservaModif);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result.Message);
        }
        [HttpGet("TurnosCancelados")] //nuevo
        public async Task<ActionResult<List<Reserva>>> TurnosCancelados()
        {
            var result = await _reservaService.TurnosCanceladosAsync();
            if (result == null) return BadRequest(new { Message = "No se encontraron reservas canceladas" });

            return Ok(result);
        }

        [HttpGet("TurnosConfirmados")] //nuevo 
        public async Task<ActionResult<List<Reserva>>> TurnosConfirmados()
        {
            var result = await _reservaService.TurnosConfirmadosAsync();
            if (result == null) return BadRequest(new { Message = "No se encontraron reservas confirmadas" });

            return Ok(result);
        }

        [HttpGet("librePorFecha")]
        public async Task<ActionResult<List<Reserva>>> GetTurnosLibresPorFecha ([FromQuery] DateTime Fecha)
            {
            var result = await _reservaService.TurnosLibresPorFechaAsync(Fecha);
            if (result == null) return BadRequest(new { Message = "No se encontraron turnos libres para esa fecha" });

            return Ok(result);
        }

    }


}
