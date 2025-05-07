using Evenda.Domain.Base;

namespace Evenda.Domain.Entities.UserEntities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new HashSet<Role>();
        public virtual ICollection<UserSession> UserSessions { get; set; } = new HashSet<UserSession>();
    }
}
