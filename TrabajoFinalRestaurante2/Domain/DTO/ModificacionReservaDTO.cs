namespace TrabajoFinalRestaurante.Domain.DTO
{
    public class ModificacionReservaDTO
    {
        public string Dni { get;set;}
        public DateTime NuevaFechaReserva { get; set; }

        public int NuevoIdRangoReserva { get; set; }

        public int CantidadPersonas { get; set; }
    }
}
