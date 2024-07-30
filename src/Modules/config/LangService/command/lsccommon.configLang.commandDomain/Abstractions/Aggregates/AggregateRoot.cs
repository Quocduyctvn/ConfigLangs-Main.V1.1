using lsccommon.configLang.commandDomain.Abstractions.Entities;

namespace lsccommon.configLang.commandDomain.Abstractions.Aggregates
{
	/// <summary>
	/// Aggregate root
	/// </summary>
	public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
	{
	}
}