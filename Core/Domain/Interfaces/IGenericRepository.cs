using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepository<TEntity, TKey>
     where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specifications);
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> Spec);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities); 
        void Update(TEntity entity);
        void Delete(TEntity entity);


    }
}
