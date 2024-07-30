using FluentValidation;
using lsccommon.configLang.commandContract.Shared;
using lsccommon.configLang.commandDomain.Abstractions.Repositories;
using lsccommon.configLang.commandDomain.Entities;
using MediatR;
using Entities = lsccommon.configLang.commandDomain.Entities;

namespace lsccommon.configLang.commandApplication.UserCases
{
	/// <summary>
	/// Request to create Lang, contain Id, description, vn, en 
	/// </summary>
	public class CreateLangCommand : IRequest<Result<Entities.Lang>>
	{
		public string Id { get; set; }
		public string? description { get; set; }
		public string vn { get; set; }
		public string? en { get; set; }
	}



	/// <summary>
	/// Validator for Create Lang command
	/// </summary>
	public class CreateLangCommandValidator : AbstractValidator<CreateLangCommand>
	{
		public CreateLangCommandValidator()
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
	/// Handler for create Lang request
	/// </summary>
	public class CreateLangCommandHandler : IRequestHandler<CreateLangCommand, Result<Entities.Lang>>
	{
		private readonly IUnitOfWork unitOfWork;

		public CreateLangCommandHandler(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}
		public async Task<Result<Lang>> Handle(CreateLangCommand request, CancellationToken cancellationToken)
		{
			var lang = Lang.TryCreate(request.Id, request.description, request.vn, request.en);
			// Begin transaction
			using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
			try
			{
				// Get Lang repository with id type string
				var langRepository = unitOfWork.Repository<Lang, string>();

				// Add data
				langRepository.Add(lang);

				// Save data
				await langRepository.SaveChangesAsync(cancellationToken);

				// Commit transaction
				transaction.Commit();
				return lang;
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