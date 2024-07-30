
using Asp.Versioning;
using lsccommon.configLang.commandApplication.UserCases;
using lsccommon.configLang.commandContract.DTOs;
using lsccommon.configLang.commandPresentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace lsccommon.configLang.commandPresentation.Controllers.v1
{
	/// <summary>
	/// Controller version 1 for Lang APIs.
	/// </summary>
	[ApiVersion(1)]  // Specify API version
	[Route("api/v{v:apiVersion}/Lang")]  // Route template with versioning
	public class LangController : ApiController
	{
		private readonly IMediator mediator;

		public LangController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		/// <summary>
		/// API endpoint for creating a Lang (version 1).
		/// </summary>
		/// <param name="command">Request to create a Lang.</param>
		/// <returns>Action result.</returns>
		[MapToApiVersion(1)]  // Maps to API version 1
		[HttpPost]  // HTTP POST method
		public async Task<IActionResult> CreateLangV1(CreateLangCommand command)
		{
			var result = await mediator.Send(command);
			if (result.IsSuccess)
			{
				return Ok(result.Value);  // Returns HTTP 200 OK with result value
			}

			return BadRequest(result.Error);  // Returns HTTP 400 Bad Request with error message
		}

		/// <summary>
		/// API endpoint for updating a Lang (version 1).
		/// </summary>
		/// <param name="id">Identifier of the Lang to update.</param>
		/// <param name="request">Request body containing content to update.</param>
		/// <returns>Action result.</returns>
		[MapToApiVersion(1)]  // Maps to API version 1
		[HttpPut]  // HTTP PUT method
		public async Task<IActionResult> UpdateLangV1(string id, [FromBody] UpdateLangRequestDTO request)
		{
			var command = new UpdateLangCommand(id, request.description, request.vn, request.vn);
			var result = await mediator.Send(command);
			if (result.IsSuccess)
			{
				return Ok(result);  // Returns HTTP 200 OK with result
			}

			return BadRequest(result.Error);  // Returns HTTP 400 Bad Request with error message
		}
	}
}