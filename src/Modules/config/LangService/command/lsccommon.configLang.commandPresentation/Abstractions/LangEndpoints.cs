using lsccommon.configLang.commandPresentation.APIs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace lsccommon.configLang.commandPresentation.Abstractions
{
	/// <summary>
	/// Contain configurations for Lang endpoints.
	/// </summary>
	public static class LangEndpoints
	{
		/// <summary>
		/// Map Lang endpoints
		/// </summary>
		/// <param name="app"></param>
		public static void MapLangEndpoints(this IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("/minimal/langs/");
			group.MapPost("", LangApiV1.CreateLangV1).MapToApiVersion(1);
			group.MapPut("{id:int}", LangApiV1.UpdateLangV1).MapToApiVersion(1);

		}
	}
}