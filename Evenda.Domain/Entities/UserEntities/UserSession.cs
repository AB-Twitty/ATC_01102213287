using Evenda.Domain.Base;

namespace Evenda.Domain.Entities.UserEntities
{
    public class UserSession : BaseEntity
    {
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
