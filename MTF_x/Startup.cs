using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MTF_x.Startup))]
namespace MTF_x
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
