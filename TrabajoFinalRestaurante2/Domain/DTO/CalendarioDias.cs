namespace TrabajoFinalRestaurante.Domain.DTO
{
    public class CalendarioDias
    {
        public DateTime Fecha { get; set; }
        public string Dia { get; set; } = string.Empty;

        public List<RangoInfo> Rangos { get; set; }
    }
}
