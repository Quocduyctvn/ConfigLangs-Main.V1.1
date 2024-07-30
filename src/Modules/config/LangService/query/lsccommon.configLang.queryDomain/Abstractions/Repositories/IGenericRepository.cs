using lsccommon.configLang.queryDomain.Abstractions.Entities;
using System.Linq.Expressions;

namespace lsccommon.configLang.queryDomain.Abstractions.Repositories
{
    /// <summary>
    /// Provide generic repository with custom type of key
    /// </summary>
    /// <typeparam name="TEntity">Generic type of domain entity</typeparam>
    /// <typeparam name="TKey">Generic type of entity key</typeparam>
    public interface IGenericRepository<TEntity, in TKey>
        where TEntity : IEntity
    {
        /// <summary>
        /// Find domain entity by id. Returned entity can be tracking
        /// </summary>
        /// <param name="id">ID of domain entity</param>
        /// <param name="isTracking">Tracking state of entity</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity with given id or null if entity with given id not found</returns>
        Task<TEntity?> FindByIdAsync(TKey id,
                                     bool isTracking = false,
                                     CancellationToken cancellationToken = default,
                                     params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Find single entity that satisfied predicate expression. Can be tracking
        /// </summary>
        /// <param name="isTracking">Tracking state of entity</param>
        /// <param name="predicate">Predicate expression</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity matched expression or null if entity not found</returns>
        Task<TEntity?> FindSingleAsync(bool isTracking = false,
                                       Expression<Func<TEntity, bool>>? predicate = null,
                                       CancellationToken cancellationToken = default,
                                       params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Find all entity that satisfied predicate expression. Can be tracking
        /// </summary>
        /// <param name="isTracking">Tracking state of entity</param>
        /// <param name="predicate">Predicate expression</param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>IQueryable of entities that match predicate expression</returns>
        IQueryable<TEntity> FindAll(bool isTracking = false,
                                    Expression<Func<TEntity, bool>>? predicate = null,
                                    params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Marked entity as Added state
        /// </summary>
        /// <param name="entity">Added entity</param>
        void Add(TEntity entity);

        /// <summary>
        /// Marked entity as Updated state
        /// </summary>
        /// <param name="entity">Updated entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Marked entity as Deleted state
        /// </summary>
        /// <param name="entity">Removed entity</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Marked multiple entities as Deleted state
        /// </summary>
        /// <param name="entities">Removed entities</param>
        void RemoveMultiple(List<TEntity> entities);

        /// <summary>
        /// Apply all changes in context to database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Number of changes are made to database</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}