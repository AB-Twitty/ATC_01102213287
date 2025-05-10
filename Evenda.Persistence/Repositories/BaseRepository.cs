using Evenda.App.Contracts.IPersistence.IRepositories;
using Evenda.App.Utils;
using Evenda.Domain.Base;
using Evenda.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Evenda.Persistence.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        #region Fields 

        protected readonly AppDbContext _context;
        private readonly DbSet<TEntity> Entities;

        #endregion

        #region Ctor

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            Entities = _context.Set<TEntity>();
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var query = Entities.AsQueryable().AsNoTracking();
            return await query.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = Entities.AsQueryable();
            return await query.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            var entity = await Entities.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentNullException($"Entity with id {id} not found");
            }
            return entity;
        }

        public async Task AddAsync(TEntity entity)
        {
            await Entities.AddAsync(entity);
        }

        public async Task<TEntity> AddAsyncWithReturn(TEntity entity)
        {
            await Entities.AddAsync(entity);

            return entity;
        }

        public async Task AddRangeAsync(List<TEntity> entity)
        {
            await Entities.AddRangeAsync(entity);
        }

        public void Update(TEntity entity)
        {
            Entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            Entities.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            Entities.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool? asNoTracking = true, int? take = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            Expression<Func<TEntity, object>>? thenOrderBy = null,
            bool orderByDescending = false)
        {
            var query = Entities.AsQueryable();

            if (asNoTracking.HasValue && asNoTracking.Value)
            {
                query = query.AsNoTracking();
            }

            if (include != null) query = include(query);

            query = query.Where(predicate);

            if (orderBy != null)
            {
                query = orderByDescending
                        ? query.OrderByDescending(orderBy)
                        : query.OrderBy(orderBy);
            }
            else if (orderBy != null && thenOrderBy != null)
            {
                query = orderByDescending
                    ? query.OrderByDescending(orderBy).ThenByDescending(thenOrderBy)
                    : query.OrderBy(orderBy).ThenBy(thenOrderBy);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<PagedList<TMapEntity>> FindPaginatedAsync<TMapEntity>(Expression<Func<TEntity, bool>> predicate,
            int pageNumber, int pageSize, Func<TEntity, TMapEntity> mapFunc,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Expression<Func<TEntity, object>>? orderBy = null, bool orderByDescending = false)
        {
            var entityList = await FindPaginatedAsync(predicate, pageNumber, pageSize, include, orderBy, orderByDescending);

            var pagedList = new PagedList<TMapEntity>(
                source: entityList.Items.Select(mapFunc).ToList(),
                pageIndex: entityList.PageIndex,
                pageSize: entityList.PageSize,
                totalCount: entityList.TotalCount
            );

            return pagedList;
        }

        public async Task<PagedList<TEntity>> FindPaginatedAsync(Expression<Func<TEntity, bool>> predicate,
            int pageNumber, int pageSize,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Expression<Func<TEntity, object>>? orderBy = null, bool orderByDescending = false)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 12 : pageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;

            int totalCount = 0;
            List<TEntity> items = [];

            var query = Entities.AsQueryable().AsNoTracking();
            if (include != null) query = include(query);

            query = query.Where(predicate);

            if (orderBy != null)
            {
                query = orderByDescending
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);
            }

            var result = await query
                .Select(e => new { TotalCount = query.Count(), Item = e })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (result.Count != 0)
            {
                totalCount = result.First().TotalCount;
                items = result.Select(r => r.Item).ToList();
            }

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedList<TEntity>(items, pageNumber, pageSize, totalCount);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            Expression<Func<TEntity, object>>? thenOrderBy = null,
            bool orderByDescending = false)
        {
            var query = Entities.AsQueryable();

            if (include != null) query = include(query);

            if (orderBy != null)
            {
                query = orderByDescending
                        ? query.OrderByDescending(orderBy)
                        : query.OrderBy(orderBy);
            }
            else if (orderBy != null && thenOrderBy != null)
            {
                query = orderByDescending
                    ? query.OrderByDescending(orderBy).ThenByDescending(thenOrderBy)
                    : query.OrderBy(orderBy).ThenBy(thenOrderBy);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate)
        {
            var query = Entities.AsQueryable();
            return await query.AnyAsync(predicate);
        }
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = Entities.AsQueryable();
            return await query.CountAsync(predicate);
        }

        #endregion
    }
}
