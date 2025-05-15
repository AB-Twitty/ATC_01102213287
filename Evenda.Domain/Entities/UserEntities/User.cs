using Evenda.Domain.Base;
using Evenda.Domain.Entities.TicketEntities;

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

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
