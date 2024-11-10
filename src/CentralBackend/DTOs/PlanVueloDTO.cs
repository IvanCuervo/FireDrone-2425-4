namespace CentralBackend.DTOs
{
    public class PlanVueloDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int ControlManual { get; set; }
        public int DronId { get; set; }
        public int RutaId { get; set; }
    }
}
