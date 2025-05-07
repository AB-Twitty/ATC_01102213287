using Evenda.App.Contracts.IPersistence.IRepositories;
using Evenda.Domain.Entities.UserEntities;
using Evenda.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Evenda.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        #region Fields

        private readonly AppDbContext _context;

        #endregion

        #region Ctor

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<User?> GetUserWithSessionsAndRolesByEmail(string email)
        {
            return await FirstOrDefaultAsync(
                userAccount => !userAccount.IsDeleted && userAccount.Email == email,
                include: source => source
                    .Include(x => x.UserSessions)
                    .Include(x => x.Roles)
                );
        }

        public async Task<User?> GetUserWithSessionsAndRolesById(string id)
        {
            return await FirstOrDefaultAsync(
                userAccount => !userAccount.IsDeleted && userAccount.Id.ToString() == id,
                include: source => source
                    .Include(x => x.UserSessions)
                    .Include(x => x.Roles)
                );
        }

        #endregion
    }
}
