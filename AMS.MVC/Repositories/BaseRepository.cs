using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AMS.MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace AMS.MVC.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<T> GetBy(Expression<Func<T, bool>> expression);
        
        IQueryable<T> GetAll();
        
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression);

        Task<bool> Exists(Expression<Func<T, bool>> expression);
        
        Task<int> Count();

        Task<int> Count(Expression<Func<T, bool>> expression);
        
        Task Create(T entity);

        void Update(T entity);

        void Delete(T entity);
    }

    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected DatabaseContext DatabaseContext { get; }

        protected BaseRepository(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        public async Task<T> GetBy(Expression<Func<T, bool>> expression)
        {
            return await DatabaseContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public IQueryable<T> GetAll()
        {
            return DatabaseContext.Set<T>();
        }
        
        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return DatabaseContext.Set<T>().Where(expression);
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> expression)
        {
            return await DatabaseContext.Set<T>().AnyAsync(expression);
        }
        
        public async Task<int> Count()
        {
            return await DatabaseContext.Set<T>().CountAsync();
        }

        public async Task<int> Count(Expression<Func<T, bool>> expression)
        {
            return await DatabaseContext.Set<T>().CountAsync(expression);
        }

        public async Task Create(T entity)
        {
            await DatabaseContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            DatabaseContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            DatabaseContext.Set<T>().Remove(entity);
        }
    }
}
