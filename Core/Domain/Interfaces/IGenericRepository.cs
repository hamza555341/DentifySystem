using Domain.Entites;

namespace Domain.Interfaces
{
    public interface IGenericRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification);

        Task<TEntity?> GetByIdAsync(TKey id);

        Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification);

        Task AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task<int> CountAsync(ISpecification<TEntity, TKey> specification);

        Task<bool> AnyAsync(ISpecification<TEntity, TKey> specification);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}