using lsccommon.configLang.queryDomain.Abstractions.Entities;
using System.Data;

namespace lsccommon.configLang.queryDomain.Abstractions.Repositories
{
    /// <summary>
    /// Interface for unit of work, can be disposable
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Get repository of generic type
        /// </summary>
        /// <typeparam name="TEntity">Generic type of domain entity</typeparam>
        /// <typeparam name="TKey">Generic key of entity</typeparam>
        /// <returns>Generic repository of entity type</returns>
        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : Entity<TKey>;

        /// <summary>
        /// Save all changes to database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Number of changes are made to database</returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Database Transaction
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Database transaction</returns>
        public Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}