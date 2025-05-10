using Evenda.Domain.Base;
using Evenda.Domain.Entities.EventEntities;

namespace Evenda.Domain.Entities.TagEntities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();
    }
}
