using Evenda.UI.Models.EventVM;

namespace Evenda.UI.Dtos.Event
{
    public class EditEventDto : CreateEventDto
    {
        public Guid Id { get; set; }
        public IList<Guid> DeletedImgIds { get; set; }

        public EditEventDto(CreateEventVM vm) : base(vm)
        {
            Id = vm.EventId ?? new Guid();
            DeletedImgIds = vm.DeletedImageIds?.Split(',')
                .Where(x => Guid.TryParse(x, out Guid _))
                .Select(x => new Guid(x)).ToList() ?? [];
        }
    }
}
