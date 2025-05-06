using Evenda.Domain.Base;

namespace Evenda.Domain.Entities.UserEntities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public string SystemName { get; set; }


        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
