using Api.GRRInnovations.Memorix.Domain.Common;

namespace Api.GRRInnovations.Memorix.Application.Interfaces.Persistence
{
    /// <summary>
    /// Generic interface for basic CRUD operations on entities that inherit from BaseModel
    /// </summary>
    /// <typeparam name="TEntity">Entity type that must inherit from BaseModel</typeparam>
    public interface IRepository<TEntity> where TEntity : BaseModel
    {
        /// <summary>
        /// Retrieves an entity by its unique identifier (Uid)
        /// </summary>
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all entities of type TEntity
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new entity to the context (does not persist, only tracks)
        /// </summary>
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks an entity as modified for update
        /// </summary>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks an entity for removal
        /// </summary>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an entity exists by its unique identifier
        /// </summary>
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an entity by its unique identifier
        /// </summary>
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

