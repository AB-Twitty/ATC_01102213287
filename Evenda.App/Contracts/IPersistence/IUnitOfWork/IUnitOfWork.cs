using Evenda.App.Contracts.IPersistence.IRepositories;
using Evenda.Domain.Base;

namespace Evenda.App.Contracts.IPersistence.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        public TICustomRepository GetCustomRepository<TICustomRepository, TEntity>()
            where TICustomRepository : class, IBaseRepository<TEntity> where TEntity : BaseEntity;
        Task BeginTransactionAsync();
        Task RollbackAsync();
        Task SaveChangesAsync();
        Task CommitAsync();
    }
}
