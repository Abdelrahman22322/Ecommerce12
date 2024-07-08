using System.Linq.Expressions;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public interface IGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Generic method to add an entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The added entity.</returns>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Generic method to add a range of entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities.</typeparam>
    /// <param name="entities">The entities to add.</param>
    /// <returns>The added entity.</returns>
    Task<TEntity> AddRangeAsync(TEntity entities);

    /// <summary>
    /// Generic method to update an entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to update.</param>
    /// <returns>The updated entity.</returns>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Generic method to delete an entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>The deleted entity.</returns>
    Task<TEntity> DeleteAsync(TEntity entity);

    /// <summary>
    /// Generic method to delete a range of entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities.</typeparam>
    /// <param name="entities">The entities to delete.</param>
    /// <returns>The deleted entity.</returns>
    Task<TEntity> DeleteRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Generic method to get an entity by its ID.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The entity with the specified ID.</returns>
    Task<TEntity> GetByIdAsync(int id);

    /// <summary>
    /// Generic method to get all entities that satisfy the specified predicate.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities.</typeparam>
    /// <param name="predicate">The predicate to filter the entities.</param>
    /// <param name="includeword">The word to include in the query.</param>
    /// <returns>The collection of entities that satisfy the predicate.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate , string? includeword);

    /// <summary>
    /// Generic method to find entities that satisfy the specified predicate.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities.</typeparam>
    /// <param name="predicate">The predicate to filter the entities.</param>
    /// <param name="includeword">The word to include in the query.</param>
    /// <returns>The collection of entities that satisfy the predicate.</returns>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, string? includeword);
    /// <summary>
    ///   Save changes to the database.
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}
