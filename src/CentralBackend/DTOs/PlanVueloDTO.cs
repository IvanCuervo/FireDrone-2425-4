namespace CentralBackend.DTOs
{
    public class PlanVueloDTO
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int ControlManual { get; set; }
        public int DronId { get; set; }
        public int RutaId { get; set; }
    }
}
