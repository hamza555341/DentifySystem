using Domain.Entites;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> :
        IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly DentifyDbContext _dbContext;

        public GenericRepository(DentifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity) =>
            await _dbContext.Set<TEntity>().AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        }

        public void Delete(TEntity entity) =>
                 _dbContext.Set<TEntity>().Remove(entity);


        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbContext.Set<TEntity>().ToListAsync();



        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> Spec)
        {
            var EntryPoint = _dbContext.Set<TEntity>();
            var Query = SpecificationsEvalutor.CreateQuery(EntryPoint, Spec);
            return await Query.ToListAsync();

        }

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specifications)
        {
            var Query = SpecificationsEvalutor.CreateQuery(_dbContext.Set<TEntity>(), specifications);
            return await Query.FirstOrDefaultAsync();

        }

        public async Task<TEntity?> GetByIdAsync(TKey id) =>
            await _dbContext.Set<TEntity>().FindAsync(id);


        public void Update(TEntity entity) =>
                 _dbContext.Set<TEntity>().Update(entity);

    }

}
