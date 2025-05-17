namespace Evenda.App.Dtos.Event
{
    public class EditEventDto : CreateEventDto
    {
        public Guid Id { get; set; }
        public IList<Guid> DeletedImgIds { get; set; } = [];
    }
}
