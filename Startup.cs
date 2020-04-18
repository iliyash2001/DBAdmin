using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DBAdmin.Startup))]
namespace DBAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
