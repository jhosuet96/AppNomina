using Microsoft.EntityFrameworkCore;
using System;
//using System.Data.Entity;

namespace AppNominas.Model
{
    public class AppNominaContext : DbContext
    {
      
        public AppNominaContext(DbContextOptions<AppNominaContext> options) : base(options) { }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Nomina> Nomina { get; set; }

        public DbSet<RetencionISR> RetencionISR { get; set; }

        public DbSet<NominaGenerada> NominaGenerada { get; set; }

        
       
        
    }
}
