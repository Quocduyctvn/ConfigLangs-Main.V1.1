using FluentValidation;
using lsccommon.configLang.queryContract.Errors;
using lsccommon.configLang.queryContract.Shared;
using lsccommon.configLang.queryDomain.Abstractions.Repositories;
using MediatR;
using Entities = lsccommon.configLang.queryDomain.Entities;

namespace lsccommon.configLang.queryApplication.UserCases
{
	/// <summary>
	/// Request to get Lang by id
	/// </summary>
	public record GetLangByIdQuery(string Id) : IRequest<Result<Entities.Lang>>
	{
	}


	/// <summary>
	/// Validator for GetLangById
	/// </summary>
	public class GetLangByIdQueryValidator : AbstractValidator<GetLangByIdQuery>
	{
		public GetLangByIdQueryValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty()
				.NotNull()
				.MaximumLength(Entities.Lang.LangIdxKeyMaximumLength)
				.Must(funcCheckUpperCase).WithMessage("Id must be in uppercase")
				.Matches(@"^[^\s]+$").WithMessage("Id must not contain any whitespace characters");
		}


		/// <summary>
		/// Functions to check capital letters
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		private bool funcCheckUpperCase(string Id)
		{
			if (string.IsNullOrEmpty(Id))
			{
				return false;
			}
			// Only check the letters, ignore other characters such as lower bricks
			return Id.Where(char.IsLetter).All(char.IsUpper);
		}
	}

	/// <summary>
	/// Handler for get Lang by id request
	/// </summary>
	public class GetLangByIdQueryHandler : IRequestHandler<GetLangByIdQuery, Result<Entities.Lang>>
	{
		private readonly IUnitOfWork unitOfWork;

		public GetLangByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Handle request
		/// </summary>
		/// <param name="request">Request to handle</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Result with Lang data</returns>
		public async Task<Result<Entities.Lang>> Handle(GetLangByIdQuery request,
														  CancellationToken cancellationToken)
		{
			try
			{
				var langRepository = unitOfWork.Repository<Entities.Lang, string>();
				var lang = await langRepository.FindByIdAsync(request.Id, cancellationToken: cancellationToken);
				if (lang is null)
				{
					return Result.Failure(
						Error.NotFound(MessageConstant.NotFound<Entities.Lang>(x => x.Id, request.Id)));
				}

				return Result.Success(lang);
			}
			catch (Exception e)
			{
				return Result.Failure(Error.ServerError(e.Message));
			}
		}
	}
}