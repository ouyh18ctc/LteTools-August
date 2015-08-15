using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using Lte.Parameters.Concrete;

namespace Lte.Parameters.Abstract
{
    public abstract class ParametersRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<EFParametersContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected ParametersRepositoryBase(IDbContextProvider<EFParametersContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }

    public abstract class ParametersRepositoryBase<TEntity> : ParametersRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected ParametersRepositoryBase(IDbContextProvider<EFParametersContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }

    public abstract class LightWeightRepositroyBase<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity<int>, new()
    {
        protected EFParametersContext context = new EFParametersContext();

        protected abstract DbSet<TEntity> Entities { get; }

        public IQueryable<TEntity> GetAll()
        {
            return Entities;
        }

        public List<TEntity> GetAllList()
        {
            return Entities.ToList();
        }

        public Task<List<TEntity>> GetAllListAsync()
        {
            return Task.Run(() => GetAllList());
        }

        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate).ToList();
        }

        public Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.Run(() => GetAllList(predicate));
        }

        public T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(Entities);
        }

        public TEntity Get(int id)
        {
            return Entities.FirstOrDefault(x => x.Id == id);
        }

        public Task<TEntity> GetAsync(int id)
        {
            return Task.Run(() => Get(id));
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Single(predicate);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.Run(() => Single(predicate));
        }

        public TEntity FirstOrDefault(int id)
        {
            return FirstOrDefault(x => x.Id == id);
        }

        public Task<TEntity> FirstOrDefaultAsync(int id)
        {
            return Task.Run(() => FirstOrDefault(id));
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.FirstOrDefault(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.Run(() => FirstOrDefault(predicate));
        }

        public TEntity Load(int id)
        {
            return new TEntity { Id = id };
        }

        public TEntity Insert(TEntity entity)
        {
            TEntity info = Entities.Add(entity);
            context.SaveChanges();
            return info;
        }

        public Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.Run(() => Insert(entity));
        }

        public int InsertAndGetId(TEntity entity)
        {
            TEntity info = Entities.Add(entity);
            context.SaveChanges();
            return info.Id;
        }

        public Task<int> InsertAndGetIdAsync(TEntity entity)
        {
            return Task.Run(() => InsertAndGetId(entity));
        }

        public TEntity InsertOrUpdate(TEntity entity)
        {
            TEntity item = Get(entity.Id);
            return (item == null) ? Insert(entity) : Update(entity);
        }

        public Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return Task.Run(() => InsertOrUpdate(entity));
        }

        public int InsertOrUpdateAndGetId(TEntity entity)
        {
            TEntity item = Get(entity.Id);
            return (item == null) ? Insert(entity).Id : Update(entity).Id;
        }

        public Task<int> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            return Task.Run(() => InsertOrUpdateAndGetId(entity));
        }

        public TEntity Update(TEntity entity)
        {
            context.SaveChanges();
            return entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.Run(() => Update(entity));
        }

        public TEntity Update(int id, Action<TEntity> updateAction)
        {
            TEntity item = Get(id);
            if (item == null) return null;
            updateAction(item);
            context.SaveChanges();
            return item;
        }

        public Task<TEntity> UpdateAsync(int id, Func<TEntity, Task> updateAction)
        {
            return Task.Run(() =>
            {
                TEntity item = Get(id);
                if (item == null) return null;
                updateAction(item);
                context.SaveChanges();
                return item;
            });
        }

        public void Delete(TEntity entity)
        {
            Entities.Remove(entity);
            context.SaveChanges();
        }

        public Task DeleteAsync(TEntity entity)
        {
            return Task.Run(() => Delete(entity));
        }

        public void Delete(int id)
        {
            TEntity item = Get(id);
            if (item != null) Delete(item);
        }

        public Task DeleteAsync(int id)
        {
            return Task.Run(() => Delete(id));
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity item = FirstOrDefault(predicate);
            if (item != null) Delete(item);
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.Run(() => Delete(predicate));
        }

        public int Count()
        {
            return GetAll().Count();
        }

        public Task<int> CountAsync()
        {
            return GetAll().CountAsync();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Count(predicate);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().CountAsync(predicate);
        }

        public long LongCount()
        {
            return GetAll().LongCount();
        }

        public Task<long> LongCountAsync()
        {
            return GetAll().LongCountAsync();
        }

        public long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCount(predicate);
        }

        public Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCountAsync(predicate);
        }
    }
}
