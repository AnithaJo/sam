using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RabbitMQWeb.Startup))]
namespace RabbitMQWeb
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
