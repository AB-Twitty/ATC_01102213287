using Evenda.App.Contracts.IPersistence.IRepositories;
using Evenda.App.Contracts.IPersistence.IUnitOfWork;
using Evenda.Domain.Base;
using Evenda.Persistence.Context;
using Evenda.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Evenda.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;
        private readonly AppDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        #endregion

        #region Ctor

        public UnitOfWork(AppDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Methods

        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            return new BaseRepository<TEntity>(_context);
        }

        public TICustomRepository GetCustomRepository<TICustomRepository, TEntity>()
            where TICustomRepository : class, IBaseRepository<TEntity> where TEntity : BaseEntity
        {
            var repo = _serviceProvider.GetService<TICustomRepository>();
            if (repo == null)
                throw new InvalidOperationException($"Repository for {typeof(TEntity).Name} not registered.");

            return repo;
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                _currentTransaction = await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync();
                }
            }
            finally
            {
                _currentTransaction = null;
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                _currentTransaction?.Commit();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                _currentTransaction = null;
            }
        }

        #endregion
    }
}
