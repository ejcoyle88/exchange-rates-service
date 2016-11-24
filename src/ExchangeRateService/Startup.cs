using Microsoft.Owin;
using ExchangeRateService;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace ExchangeRateService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureNancy(app);
        }
    }
}