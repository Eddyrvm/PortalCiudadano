using PortalCiudadano.Models.LiquidacionPatente;
using PortalCiudadano.Models.MapaSitios;
using PortalCiudadano.Models.ServiciosPublicos;
using System.Data.Entity;

namespace PortalCiudadano.Models
{
    public class PortalCiudadanoContext : DbContext
    {
        public PortalCiudadanoContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<TipoServicio> TipoServicios { get; set; }

        public DbSet<ServicioLimpeza> ServicioLimpezas { get; set; }

        public DbSet<MapSitio> MapSitios { get; set; }

        public DbSet<PersonaJuridica> PersonaJuridicas { get; set; }

        public DbSet<PersonaNatural> PersonaNaturales { get; set; }

        public DbSet<Clasificacion> Clasificaciones { get; set; }

        public DbSet<Actividad> Actividades { get; set; }

        public DbSet<InfoEstadisticaProduc> InfoEstadisticaProducs { get; set; }

        public DbSet<CantidadEmpleado> CantidadEmpleados { get; set; }

        public DbSet<LiquidarPatentePN> LiquidarPatentePNs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonaNatural>()
                .Property(p => p.CapitalPropio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PersonaJuridica>()
                .Property(p => p.CapitalPropio)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}