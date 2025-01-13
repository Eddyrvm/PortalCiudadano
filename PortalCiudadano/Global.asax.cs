using PortalCiudadano.Clases;
using PortalCiudadano.Models;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PortalCiudadano
{
    public class MvcApplication : HttpApplication
    {

        protected void Application_Start()
        {
            CheckRolesAndSuperUser();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PortalCiudadanoContext, Migrations.Configuration>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        private void CheckRolesAndSuperUser()
        {
            UsersHelper.CheckRole("Admin");
            UsersHelper.CheckRole("Ver");
            UsersHelper.CheckRole("Editar");
            UsersHelper.CheckRole("Eliminar");
            UsersHelper.CheckRole("Modificar");
            UsersHelper.CheckSuperUser();
        }
    }
}
