using Evenda.App.Utils;
using Evenda.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Evenda.App.Contracts.IPersistence.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetByIdAsync(Guid id);
        Task AddAsync(TEntity entity);
        Task<TEntity> AddAsyncWithReturn(TEntity entity);
        Task AddRangeAsync(List<TEntity> entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool? asNoTracking = true, int? take = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            Expression<Func<TEntity, object>>? thenOrderBy = null,
            bool orderByDescending = false);
        Task<PagedList<TMapEntity>> FindPaginatedAsync<TMapEntity>(Expression<Func<TEntity, bool>> predicate,
            int pageNumber, int pageSize, Func<TEntity, TMapEntity> mapFunc,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Expression<Func<TEntity, object>>? orderBy = null, bool orderByDescending = false);
        Task<PagedList<TEntity>> FindPaginatedAsync(Expression<Func<TEntity, bool>> predicate,
            int pageNumber, int pageSize,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Expression<Func<TEntity, object>>? orderBy = null, bool orderByDescending = false);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            Expression<Func<TEntity, object>>? thenOrderBy = null,
            bool orderByDescending = false);
        Task<bool> Exists(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        DbSet<TEntity> Table();
    }
}
