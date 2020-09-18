using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HSE.Startup))]
namespace HSE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
