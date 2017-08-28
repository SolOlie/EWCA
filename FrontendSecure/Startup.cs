using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FrontendSecure.Startup))]
namespace FrontendSecure
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
