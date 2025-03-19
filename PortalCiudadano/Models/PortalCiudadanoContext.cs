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

        public System.Data.Entity.DbSet<PortalCiudadano.Models.ServiciosPublicos.ServicioLimpeza> ServicioLimpezas { get; set; }
        public DbSet<MapSitio> MapSitios { get; set; }
    }
}