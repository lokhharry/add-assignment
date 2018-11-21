using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(add_assignment.Startup))]
namespace add_assignment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
