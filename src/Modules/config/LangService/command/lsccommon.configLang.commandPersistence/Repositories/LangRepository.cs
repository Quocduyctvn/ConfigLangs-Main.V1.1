using lsccommon.configLang.commandDomain.Abstractions.Repositories;
using lsccommon.configLang.commandDomain.Entities;

namespace lsccommon.configLang.commandPersistence.Repositories
{
	/// <summary>
	/// Implementation of ILangRepository
	/// </summary>
	public class LangRepository(ApplicationDbContext context)
		: GenericRepository<Lang, string>(context), ILangRepository;
}