using lsccommon.configLang.queryPresentation.APIs.Langs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace lsccommon.configLang.queryPresentation.Abstractions
{
	/// <summary>
	/// Contain configurations for Lang endpoints.
	/// </summary>
	public static class LangEndpoints
	{
		/// <summary>
		/// Map Lang endpoints
		/// </summary>
		/// <param name="app">dependency injection IEndpointRouteBuilder</param>
		public static void MapLangEndpoints(this IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("minimal/langs/");
			group.MapGet("{id}", LangApiV1.GetLangByIdV1).MapToApiVersion(1);
			group.MapGet("", LangApiV1.GetAllLangV1).MapToApiVersion(1);
		}
	}
}