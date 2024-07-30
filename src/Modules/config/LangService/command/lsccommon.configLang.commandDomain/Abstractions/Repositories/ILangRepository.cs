using lsccommon.configLang.commandDomain.Entities;

namespace lsccommon.configLang.commandDomain.Abstractions.Repositories
{
	/// <summary>
	/// Provide Lang repository
	/// </summary>
	public interface ILangRepository : IGenericRepository<Lang, string>
	{
	}
}