using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PortalCiudadano.Startup))]
namespace PortalCiudadano
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
