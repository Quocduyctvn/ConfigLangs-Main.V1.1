
using FluentValidation;
using lsccommon.configLang.commandContract.Errors;
using lsccommon.configLang.commandContract.Shared;
using lsccommon.configLang.commandDomain.Abstractions.Repositories;
using MediatR;
using Entities = lsccommon.configLang.commandDomain.Entities;

namespace lsccommon.configLang.commandApplication.UserCases
{

	/// <summary>
	/// Request to update Lang, contain Lang: Id, description, vn, en
	/// </summary>
	public class UpdateLangCommand(string id, string? desc, string vn, string? en) : IRequest<Result>
	{
		public string Id { get; set; } = id;
		public string? description { get; set; } = desc;
		public string vn { get; set; } = vn;
		public string? en { get; set; } = en;
	}

	/// <summary>
	/// Validator for update Lang command
	/// </summary>
	public class UpdateLangCommandValidator : AbstractValidator<UpdateLangCommand>
	{
		public UpdateLangCommandValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty()
				.NotNull()
				.MaximumLength(Entities.Lang.LangIdxKeyMaximumLength)
				.Must(funcCheckUpperCase).WithMessage("Id must be in uppercase")
				.Matches(@"^[^\s]+$").WithMessage("Id must not contain any whitespace characters");

			RuleFor(x => x.description).MaximumLength(Entities.Lang.LangDescriptionMaximumLength);
			RuleFor(x => x.vn).NotEmpty().NotNull().MaximumLength(Entities.Lang.LangVnMaximumLength);
			RuleFor(x => x.en).MaximumLength(Entities.Lang.LangEnMaximumLength);
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
	/// Handler for update Lang request
	/// </summary>
	public class UpdateLangCommandHandler : IRequestHandler<UpdateLangCommand, Result>
	{
		private readonly ILangRepository _langRepository;

		public UpdateLangCommandHandler(ILangRepository langRepository)
		{
			this._langRepository = langRepository;
		}

		public async Task<Result> Handle(UpdateLangCommand request, CancellationToken cancellationToken)
		{
			using var transaction = await _langRepository.BeginTransactionAsync(cancellationToken);
			try
			{
				// Need tracking to update lang
				var lang = await _langRepository.FindByIdAsync(request.Id, true, cancellationToken);
				if (lang is null)
				{
					var messages = MessageConstant.NotFound<Entities.Lang>(x => x.Id, request.Id);
					return Result.Failure(Error.NotFound(messages));
				}

				// map dữ liệu 
				lang.Id = request.Id;
				lang.description = request.description;
				lang.vn = request.vn;
				lang.en = request.en;

				_langRepository.Update(lang);
				await _langRepository.SaveChangesAsync(cancellationToken);

				// Commit transaction
				transaction.Commit();
				return Result.Success();
			}
			catch (Exception)
			{
				// Rollback transaction
				transaction.Rollback();
				throw;
			}
		}
	}
}