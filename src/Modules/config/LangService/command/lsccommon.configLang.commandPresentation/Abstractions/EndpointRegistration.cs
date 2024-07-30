using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace lsccommon.configLang.commandPresentation.Abstractions
{
	public static class EndpointRegistration
	{
		/// <summary>
		/// Map presentation layer endpoints
		/// </summary>
		/// <param name="app"></param>
		/// <returns>The endpoint route builder with mapped presentation layer endpoints</returns>
		public static IEndpointRouteBuilder MapPresentation(this IEndpointRouteBuilder app)
		{
			app.MapControllers();
			var apiVersionSet = app.NewApiVersionSet().HasApiVersion(new ApiVersion(1)).ReportApiVersions().Build();

			//create api version prefix group
			var versionGroup = app.MapGroup("api/v{apiVersion:apiVersion}").WithApiVersionSet(apiVersionSet);
			versionGroup.MapLangEndpoints();
			return app;
		}
	}
}