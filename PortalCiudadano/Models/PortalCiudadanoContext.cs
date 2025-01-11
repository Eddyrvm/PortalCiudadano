using System.Data.Entity;

namespace PortalCiudadano.Models
{
    public class PortalCiudadanoContext : DbContext
    {
        public PortalCiudadanoContext() : base("DefaultConnection")
        {

        }

        public System.Data.Entity.DbSet<PortalCiudadano.Models.User> Users { get; set; }
    }
}