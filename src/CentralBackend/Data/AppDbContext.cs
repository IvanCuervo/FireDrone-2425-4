﻿using Microsoft.EntityFrameworkCore;
using Models;

namespace CentralBackend.Data {
    public class AppDbContext : DbContext
    {
        
        public DbSet<Area> Areas { get; set; }
        public DbSet<Dron> Drones { get; set; }
        public DbSet<EstacionBase> EstacionesBase { get; set; }
        public DbSet<EstacionControl> EstacionesControl { get; set; }
        public DbSet<Incidencia> Incidencias { get; set; }
        public DbSet<MedicionPlanVuelo> MedicionesPlanVuelo { get; set; }
        public DbSet<PlanVuelo> PlanesVuelo { get; set; }
        public DbSet<PuntoRuta> PuntosRuta { get; set; }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<PuntoPlanVuelo> PuntosPlanVuelo { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
