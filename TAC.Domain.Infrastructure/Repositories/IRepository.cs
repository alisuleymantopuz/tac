using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TAC.Domain.Infrastructure.Repositories
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetById(int id);
        Task<TEntity> GetByFilter(Expression<Func<TEntity, bool>> filter);
        Task<IList<TEntity>> ListByFilter(Expression<Func<TEntity, bool>> filter);
        Task Create(TEntity entity);
        Task Update(int id, TEntity entity);
        Task Delete(int id);
    }
}
