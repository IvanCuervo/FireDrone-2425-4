namespace Models
{

    public class Area
    {
        public int AreaId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public ICollection<EstacionBase> EstacionesBase { get; set; }
        public ICollection<EstacionControl> EstacionesControl { get; set; }
        public ICollection<Ruta> Rutas { get; set; }
    }

    public class Dron
    {
        public int DronId { get; set; }
        public string? Modelo { get; set; }
        public string? Camara { get; set; }
        public double Velocidad { get; set; }
        public double AutonomiaVuelo { get; set; }
        public double TiempoRecarga { get; set; }
        public string? Simulador { get; set; }
        public string? Estado { get; set; }
        public string? Sensores { get; set; }

        public int EstacionBaseId { get; set; }
        public EstacionBase EstacionBase { get; set; }

        public int EstacionControlId { get; set; }
        public EstacionControl EstacionControl { get; set; }

        public ICollection<PlanVuelo> PlanesVuelo { get; set; }
    }

    public class EstacionBase
    {
        public int EstacionBaseId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public int AreaId { get; set; }
        public Area Area { get; set; }

        public ICollection<Dron> Drones { get; set; }
    }

    public class EstacionControl
    {
        public int EstacionControlId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public int AreaId { get; set; }
        public Area Area { get; set; }

        public ICollection<Dron> Drones { get; set; }
    }

    public class Incidencia
    {
        public int IncidenciaId { get; set; }
        public string? Informacion { get; set; }
        public DateTime Fecha { get; set; }
        public string? Hora { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public int PlanVueloId { get; set; }
        public PlanVuelo PlanVuelo { get; set; }
    }

    public class MedicionPlanVuelo
    {
        public int MedicionPlanVueloId { get; set; }
        public DateTime Fecha { get; set; }
        public string? Hora { get; set; }
        public string? ImagenTermica { get; set; }
        public string? ImagenNormal { get; set; }
        public int Humedad { get; set; }
        public double Temperatura { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Altura { get; set; }
        public double Velocidad { get; set; }
        public string? ModoDeVuelo { get; set; }
        public string? SensoresActivados { get; set; }

        public int PlanVueloId { get; set; }
        public PlanVuelo PlanVuelo { get; set; }
    }

    public class PlanVuelo
    {
        public int PlanVueloId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int ControlManual { get; set; }

        public int DronId { get; set; }
        public Dron Dron { get; set; }

        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }

        public ICollection<Incidencia> Incidencias { get; set; }
        public ICollection<MedicionPlanVuelo> MedicionesPlanVuelo { get; set; }
    }

    public class PuntoRuta
    {
        public int PuntoRutaId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Secuencial { get; set; }

        public int RutaId { get; set; }
        public Ruta Ruta { get; set; }
    }

    public class Ruta
    {
        public int RutaId { get; set; }
        public string? Estado { get; set; }
        public string? Riesgo { get; set; }
        public string? Periodica { get; set; }
        public int NumeroPeriodicidad { get; set; }

        public int AreaId { get; set; }
        public Area Area { get; set; }

        public ICollection<PuntoRuta> PuntosRuta { get; set; }
    }
}