using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(Trippin_Website.Startup))]
namespace Trippin_Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }
    }
}
