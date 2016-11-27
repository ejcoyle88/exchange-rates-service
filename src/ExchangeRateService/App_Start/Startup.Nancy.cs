using Nancy;
using Nancy.Owin;
using Owin;

namespace ExchangeRateService
{
	public partial class Startup
	{
		private static void ConfigureNancy(IAppBuilder app)
		{
			app.UseNancy(options => options.PassThroughWhenStatusCodesAre(HttpStatusCode.NotFound));
			StaticConfiguration.DisableErrorTraces = false;
		}
	}
}