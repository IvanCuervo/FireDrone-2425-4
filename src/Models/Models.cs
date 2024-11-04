namespace Models;

public class Area
{
    public int idArea { get; set; }
    public double coordenadas { get; set; }
}

public class Dron
{
    public int idDron { get; set; }
    public string? modelo { get; set; }
    public string? camara { get; set; }
    public double velocidad { get; set; }
    public double autonomiaVuelo { get; set; }
    public double tiempoRecarga { get; set; }
    public string? simulador { get; set; }
    public string? estado { get; set; }
    public string? sensores { get; set; }
    public int idEstacionBase { get; set; }
    public int idEstacionControl { get; set; }
}

public class EstacionBase
{
    public int idEstacionBase { get; set; }
    public double coordenadas { get; set; }
    public int idArea { get; set; }
}

public class EstacionControl
{
    public int idEstacionControl { get; set; }
    public double coordenadas { get; set; }
    public int idArea { get; set; }
}

public class Incidencia
{
    public int idIncidencia { get; set; }
    public string? informacion { get; set; }
    public string? fecha { get; set; }
    public string? hora { get; set; }
    public double coordenadas { get; set; }
}

public class MedicionPlanVuelo
{
    public int idMedicionPlanVuelo { get; set; }
    public string? fecha { get; set; }
    public string? hora { get; set; }
    public string? imagenTermica { get; set; }
    public string? imagenNormal { get; set; }
    public int humedad { get; set; }
    public double temperatura { get; set; }
    public string? coordenadas { get; set; }
    public double altura { get; set; }
    public double velocidad { get; set; }
    public string? modoDeVuelo { get; set; }
    public string? sensoresActivados { get; set; }
}

public class PlanVuelo
{
    public int idPlanVuelo { get; set; }
    public string? fechaIni { get; set; }
    public string? fechaFin { get; set; }
    public int controlManual { get; set; }
    public int idDron { get; set; }
    public int idRuta { get; set; }
}

public class PuntoPlanVuelo
{
    public int idPuntoPlanVuelo { get; set; }
    public double? coordenadas { get; set; }
    public int? secuencial { get; set; }
}

public class PuntoRuta
{
    public int idPuntoRuta { get; set; }
    public double coordenadas { get; set; }
    public int secuencial { get; set; }
}

public class Ruta
{
    public int idRuta { get; set; }
    public string? estado { get; set; }
    public string? riesgo { get; set; }
    public string? periodica { get; set; }
    public int numeroPeriodico { get; set; }
    public int idArea { get; set; }
}