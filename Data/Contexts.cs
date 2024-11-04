using Microsoft.EntityFrameworkCore;


public class AreaContext : DbContext
{
    public DbSet<Model.Area> Areas { get; set; }

    public AreaContext(DbContextOptions<AreaContext> options)
        : base(options)
    {
    }
}

public class DronContext : DbContext
{
    public DbSet<Model.Dron> Drones { get; set; }

    public DronContext(DbContextOptions<DronContext> options)
        : base(options)
    {
    }
}

public class EstacionBaseContext : DbContext
{
    public DbSet<Model.EstacionBase> EstacionesBase { get; set; }

    public EstacionBaseContext(DbContextOptions<EstacionBaseContext> options)
        : base(options)
    {
    }
}

public class EstacionControlContext : DbContext
{
    public DbSet<Model.EstacionControl> EstacionesControl { get; set; }

    public EstacionControlContext(DbContextOptions<EstacionControlContext> options)
        : base(options)
    {
    }
}

public class IncidenciaContext : DbContext
{
    public DbSet<Model.Incidencia> Incidencias { get; set; }

    public IncidenciaContext(DbContextOptions<IncidenciaContext> options)
        : base(options)
    {
    }
}

public class MedicionPlanVueloContext : DbContext
{
    public DbSet<Model.MedicionPlanVuelo> MedicionesPlanVuelo { get; set; }

    public MedicionPlanVueloContext(DbContextOptions<MedicionPlanVueloContext> options)
        : base(options)
    {
    }
}

public class PlanVueloContext : DbContext
{
    public DbSet<Model.PlanVuelo> PlanesVuelo { get; set; }

    public PlanVueloContext(DbContextOptions<PlanVueloContext> options)
        : base(options)
    {
    }
}

public class PuntoPlanVueloContext : DbContext
{
    public DbSet<Model.PuntoPlanVuelo> PuntosPlanVuelo { get; set; }

    public PuntoPlanVueloContext(DbContextOptions<PuntoPlanVueloContext> options)
        : base(options)
    {
    }
}

public class PuntoRutaContext : DbContext
{
    public DbSet<Models.PuntoRuta> PuntosRuta { get; set; }

    public PuntoRutaContext(DbContextOptions<PuntoRutaContext> options)
        : base(options)
    {
    }
}

public class RutaContext : DbContext
{
    public DbSet<Model.Ruta> Rutas { get; set; }

    public RutaContext(DbContextOptions<RutaContext> options)
        : base(options)
    {
    }
}