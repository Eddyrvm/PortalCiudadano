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
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PortalCiudadanoContext, Migrations.Configuration>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
