using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace ExchangeRateService
{
	/// <summary>
	/// Documentation: https://github.com/NancyFx/Nancy/wiki/Bootstrapper
	/// </summary>
	public class Bootstrapper : DefaultNancyBootstrapper
	{
		protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
		{
			base.ApplicationStartup(container, pipelines);

			// Add modules here such as:
			// - Nancy.Authentication.Forms
			// - Nancy.Authentication.Stateless
		}

		protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
		{
			base.RequestStartup(container, pipelines, context);

			// You can also add modules here if you need to.
		}

		protected override void ConfigureConventions(NancyConventions nancyConventions)
		{
			base.ConfigureConventions(nancyConventions);

			// If you are only using Nancy as an API you should probably uncomment the following code to set responses to JSON
			//Conventions.AcceptHeaderCoercionConventions.Add((acceptHeaders, ctx) => new[]
			//{
			//    new Tuple<string, decimal>("application/json", 1)
			//}.Concat(acceptHeaders));
		}

		protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
		{
			base.ConfigureRequestContainer(container, context);

			// Register the JSON Serializer you want to use
			//container.Register<JsonSerializer, MyCustomSerializer>();

			// Register any DbContexts
			//container.Register<IUnitOfWork>((x, y) => new UnitOfWork(new AppContext()));
		}
	}
}