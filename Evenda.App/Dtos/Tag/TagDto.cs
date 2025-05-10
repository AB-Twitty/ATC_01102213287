using TagEntity = Evenda.Domain.Entities.TagEntities.Tag;

namespace Evenda.App.Dtos.Tag
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int EventsCnt { get; set; }

        public TagDto(TagEntity @tag, int cnt = 0)
        {
            Id = tag.Id;
            Name = tag.Name;

            EventsCnt = cnt;
        }
    }
}
