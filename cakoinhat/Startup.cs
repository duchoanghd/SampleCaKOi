using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cakoinhat.Startup))]
namespace cakoinhat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
