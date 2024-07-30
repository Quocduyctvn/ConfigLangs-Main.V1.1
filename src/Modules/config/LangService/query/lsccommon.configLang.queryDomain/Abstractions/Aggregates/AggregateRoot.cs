using lsccommon.configLang.queryDomain.Abstractions.Entities;

namespace lsccommon.configLang.queryDomain.Abstractions.Aggregates
{
    /// <summary>
    /// Aggregate root with custom type of key
    /// </summary>
    /// <typeparam name="TKey">Generic type</typeparam>
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
    {
    }
}