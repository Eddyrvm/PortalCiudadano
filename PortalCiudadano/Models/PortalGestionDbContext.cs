using System.Data.Entity;

namespace PortalCiudadano.Models
{
    public class PortalGestionDbContext : DbContext
    {
        public PortalGestionDbContext() : base("SecondConnection")
        {

        }

        public System.Data.Entity.DbSet<PortalCiudadano.Models.PortalGestion.UsuarioSolicita> UsuarioSolicitas { get; set; }

        public System.Data.Entity.DbSet<PortalCiudadano.Models.PortalGestion.SolicitudGestion> SolicitudGestions { get; set; }
    }
}