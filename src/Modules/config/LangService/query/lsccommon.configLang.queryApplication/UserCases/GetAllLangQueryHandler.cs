using lsccommon.configLang.queryContract.Errors;
using lsccommon.configLang.queryContract.Shared;
using lsccommon.configLang.queryDomain.Abstractions.Repositories;
using lsccommon.configLang.queryDomain.Entities;
using MediatR;
using Entities = lsccommon.configLang.queryDomain.Entities;

namespace lsccommon.configLang.queryApplication.UserCases
{
	/// <summary>
	/// Request to get all Lang
	/// </summary>
	public class GetAllLangQuery : IRequest<Result<List<Entities.Lang>>>
	{
	}

	/// <summary>
	/// Handler for get all Lang request
	/// </summary>
	public class GetAllLangQueryHandler : IRequestHandler<GetAllLangQuery, Result<List<Entities.Lang>>>
	{
		private readonly IUnitOfWork unitOfWork;

		public GetAllLangQueryHandler(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Handle request
		/// </summary>
		/// <param name="request">Request to handle</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Result with list Lang as data</returns>
		public async Task<Result<List<Lang>>> Handle(GetAllLangQuery request, CancellationToken cancellationToken)
		{
			try
			{
				var langRepository = unitOfWork.Repository<Entities.Lang, string>();
				var lang = langRepository.FindAll().ToList();
				return await Task.FromResult(Result.Success(lang));
			}
			catch (Exception e)
			{
				return Result.Failure(Error.ServerError(e.Message));
			}
		}
	}
}