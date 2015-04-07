using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cards.Startup))]
namespace Cards
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
