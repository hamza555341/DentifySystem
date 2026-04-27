using Domain.Entites;
using Domain.Interfaces;
using Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DentifyDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];
        public UnitOfWork(DentifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var EntityType = typeof(TEntity);

            if (_repositories.TryGetValue(EntityType, out object? Repo))
                return (IGenericRepository<TEntity, TKey>)Repo;

            var NewRepo = new GenericRepository<TEntity, TKey>(_dbContext);

            _repositories[EntityType] = NewRepo;
            return NewRepo;

        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
