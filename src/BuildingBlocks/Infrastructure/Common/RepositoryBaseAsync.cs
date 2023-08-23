using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class RepositoryBaseAsync<T, K, TContext> : IRepositoryBaseAsync<T, K, TContext>
        where T : EntityBase<K>
        where TContext : DbContext
    {
        private TContext _dbContext;
        private IUnitOfWork<TContext> _unitOfWork;
        
        public RepositoryBaseAsync(TContext dbContext, IUnitOfWork<TContext> unitOfWork)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));   
        }

        public Task<IDbContextTransaction> BeginTransactionAsync() => _dbContext.Database.BeginTransactionAsync();

        public async Task EndTransactionAsync()
        {
            await SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();
        }

        public Task RollbackTransactionAsync() => _dbContext.Database.RollbackTransactionAsync();

        public void Create(T entity) => _dbContext.Set<T>().Add(entity);

        public async Task<K> CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await SaveChangesAsync();
            return entity.Id;
        }

        public IList<K> CreateList(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
            return entities.Select(x => x.Id).ToList();
        }

        public async Task<IList<K>> CreateListAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await SaveChangesAsync();
            return entities.Select(x => x.Id).ToList();
        }

        public void Update(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Unchanged) return;

            T exist = _dbContext.Set<T>().Find(entity.Id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Unchanged) return;

            T exist = _dbContext.Set<T>().Find(entity.Id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            await SaveChangesAsync();
        }

        public void UpdateList(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                Update(entity);
        }

        public async Task UpdateListAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                Update(entity);
            await SaveChangesAsync();
        }

        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await SaveChangesAsync();
        }

        public void DeleteList(IEnumerable<T> entities) => _dbContext.Set<T>().RemoveRange(entities);

        public async Task DeleteListAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync() => await _unitOfWork.CommitAsync();

        public IQueryable<T> FindAll(bool trackChanges = false) =>
        !trackChanges ? _dbContext.Set<T>().AsNoTracking() :
            _dbContext.Set<T>();

        public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var items = FindAll(trackChanges);
            items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
            return items;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
            !trackChanges
                ? _dbContext.Set<T>().Where(expression).AsNoTracking()
                : _dbContext.Set<T>().Where(expression);

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var items = FindByCondition(expression, trackChanges);
            items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
            return items;
        }

        public async Task<T?> GetByIdAsync(K id) =>
            await FindByCondition(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();

        public async Task<T?> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties) =>
            await FindByCondition(x => x.Id.Equals(id), trackChanges: false, includeProperties)
                .FirstOrDefaultAsync();
    }
}
