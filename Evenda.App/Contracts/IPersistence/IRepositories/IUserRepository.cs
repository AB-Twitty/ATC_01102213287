using Evenda.Domain.Entities.UserEntities;

namespace Evenda.App.Contracts.IPersistence.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetUserWithSessionsAndRolesByEmail(string email);
        Task<User?> GetUserWithSessionsAndRolesById(string id);
    }
}
