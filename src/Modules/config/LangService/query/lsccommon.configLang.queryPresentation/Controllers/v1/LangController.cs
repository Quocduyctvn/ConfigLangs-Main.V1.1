using Asp.Versioning;
using lsccommon.configLang.queryApplication.UserCases;
using lsccommon.configLang.queryPresentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace lsccommon.configLang.queryPresentation.Controllers.v1
{
	/// <summary>
	/// Controller version 1 for Lang apis
	/// </summary>
	[ApiVersion(1)]
	[Route("api/v{v:apiVersion}/Lang")]
	public class LangController : ApiController
	{
		private readonly IMediator mediator;

		public LangController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		/// <summary>
		/// Api version 1 for get Lang by id
		/// </summary>
		/// <param name="id">ID of Lang</param>
		/// <returns>Action result with Lang as data</returns>
		[MapToApiVersion(1)]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetLangByIdV1(string id)
		{
			var query = new GetLangByIdQuery(id);
			var result = await mediator.Send(query);
			if (result.IsSuccess)
			{
				return Ok(result.Value);
			}

			return BadRequest(result.Error);
		}

		/// <summary>
		/// Api version 1 for get all Lang
		/// </summary>
		/// <returns>Action result with list of Lang as data</returns>
		[MapToApiVersion(1)]
		[HttpGet]
		public async Task<IActionResult> GetAllLangV1()
		{
			var query = new GetAllLangQuery();
			var result = await mediator.Send(query);
			if (result.IsSuccess)
			{
				return Ok(result.Value);
			}

			return BadRequest(result.Error);
		}
	}
}