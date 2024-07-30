using lsccommon.configLang.commandDomain.Abstractions.Entities;
using lsccommon.configLang.commandDomain.Abstractions.Repositories;
using lsccommon.configLang.commandDomain.Exceptions;
using lsccommon.configLang.commandPersistence.Repositories;
using System.Collections;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace lsccommon.configLang.commandPersistence
{
    /// <summary>
    /// Implementation of IUnitOfWork
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly ApplicationDbContext dbContext;

        /// <summary>
        /// Use to store generic repository
        /// </summary>
        private Hashtable? repositories;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Get repository of generic type
        /// </summary>
        /// <typeparam name="TEntity">Generic type of domain entity</typeparam>
        /// <typeparam name="TKey">Generic key of entity</typeparam>
        /// <returns>Generic repository of entity type</returns>
        /// <exception cref="CreateGenericRepositoryFailedException">Throw when failed to create generic repository</exception>
        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : Entity<TKey>
        {
            // Initialize the repositories hashtable if it is null
            repositories ??= new Hashtable();
            // Get the name of the entity type
            var type = typeof(TEntity).Name;
            if (!repositories.ContainsKey(type))
            {
                try
                {
                    // Create a new instance of the generic repository
                    var repositoryType = typeof(GenericRepository<,>);
                    var repositoryInstance =
                        Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(TKey)),
                            dbContext);
                    // Add the repository instance to the hashtable
                    repositories.Add(type, repositoryInstance);
                }
                catch (Exception)
                {
                    // Throw a custom exception if repository creation fails
                    throw new CreateGenericRepositoryFailedException();
                }
            }

            return (IGenericRepository<TEntity, TKey>)repositories[type]!;
        }

        /// <summary>
        /// Save all changes to database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Number of changes are made to database</returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Database Transaction
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Database transaction</returns>
        public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            return transaction.GetDbTransaction();
        }
    }
}