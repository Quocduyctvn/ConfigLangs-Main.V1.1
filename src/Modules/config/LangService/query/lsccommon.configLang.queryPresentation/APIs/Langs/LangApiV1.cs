using lsccommon.configLang.queryApplication.UserCases;
using lsccommon.configLang.queryPresentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace lsccommon.configLang.queryPresentation.APIs.Langs
{
	/// <summary>
	/// Api version 1 of Lang
	/// </summary>
	public class LangApiV1 : ApplicationApi
	{
		/// <summary>
		/// Api version 1 for get Lang by id, use for minimal API
		/// </summary>
		/// <param name="id">ID of Lang</param>
		/// <param name="mediator">Mediator to mediate request</param>
		/// <returns>Action result with Lang as data</returns>
		public static async Task<IResult> GetLangByIdV1(string id, IMediator mediator)
		{
			var query = new GetLangByIdQuery(id);
			var result = await mediator.Send(query);
			if (result.IsSuccess)
			{
				return TypedResults.Ok(result.Value);
			}

			return TypedResults.BadRequest(result.Error);
		}

		/// <summary>
		/// Api version 1 for get Lang by id, use for minimal API
		/// </summary>
		/// <param name="id">ID of Lang</param>
		/// <param name="mediator">Mediator to mediate request</param>
		/// <returns>Action result with Lang as data</returns>
		public static async Task<IResult> GetAllLangV1(IMediator mediator)
		{
			var query = new GetAllLangQuery();
			var result = await mediator.Send(query);
			if (result.IsSuccess)
			{
				return TypedResults.Ok(result.Value);
			}

			return TypedResults.BadRequest(result.Error);
		}
	}
}