namespace ControlBackend.Servicios
{
    public class MedicionPlanVueloDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Speed { get; set; }
        public double Battery { get; set; }
        public int State { get; set; }
    }
}