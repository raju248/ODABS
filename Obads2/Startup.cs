using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Obads2.Startup))]
namespace Obads2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
