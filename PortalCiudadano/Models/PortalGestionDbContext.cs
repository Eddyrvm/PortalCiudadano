using PortalCiudadano.Models.MapaSitios;
using PortalCiudadano.Models.PortalGestion;
using System.Data.Entity;

namespace PortalCiudadano.Models
{
    public class PortalGestionDbContext : DbContext
    {
        public PortalGestionDbContext() : base("SecondConnection")
        {

        }

        public System.Data.Entity.DbSet<PortalCiudadano.Models.PortalGestion.UsuarioSolicita> UsuarioSolicitas { get; set; }

        public DbSet<SolicitudGestion> SolicitudGestions { get; set; }
    }
}