using Evenda.App.Contracts;
using Evenda.App.Contracts.IPersistence.IRepositories;
using Evenda.App.Contracts.IPersistence.IUnitOfWork;
using Evenda.App.Contracts.IServices.IEvent;
using Evenda.App.Contracts.IServices.IMedia;
using Evenda.App.Contracts.IServices.ITag;
using Evenda.App.Contracts.IValidators;
using Evenda.App.Dtos.Event;
using Evenda.App.Dtos.Media;
using Evenda.App.Models;
using Evenda.App.Utils;
using Evenda.Domain.Entities.MediaEntities;
using Evenda.Domain.Entities.TicketEntities;
using Evenda.Services.Services.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EventEntity = Evenda.Domain.Entities.EventEntities.Event;
using TagEntity = Evenda.Domain.Entities.TagEntities.Tag;

namespace Evenda.App.Services.Event
{
    public class EventService : BaseService, IEventService
    {
        #region Fields

        private readonly IValidatorDispatcher _validatorDispatcher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<EventEntity> _eventRepo;
        private readonly IBaseRepository<Image> _imageRepo;
        private readonly IBaseRepository<Ticket> _ticketRepo;
        private readonly ITagService _tagService;
        private readonly IImageService _imageService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public EventService(IUnitOfWork unitOfWork, ITagService tagService, IImageService imageService, IValidatorDispatcher validatorDispatcher, IWorkContext workContext)
        {
            _unitOfWork = unitOfWork;
            _eventRepo = _unitOfWork.GetRepository<EventEntity>();
            _imageRepo = _unitOfWork.GetRepository<Image>();
            _ticketRepo = _unitOfWork.GetRepository<Ticket>();
            _tagService = tagService;
            _imageService = imageService;
            _validatorDispatcher = validatorDispatcher;
            _workContext = workContext;
        }

        #endregion

        #region Utils

        protected virtual Expression<Func<EventEntity, object>> GenerateSortingExpression(string? sort)
        {
            var allowedSorts = new[] { "date_time", "name", "price", "tickets_cnt", "booked_cnt", "latest" };

            if (string.IsNullOrWhiteSpace(sort) || !allowedSorts.Contains(sort))
            {
                sort = allowedSorts[0];
            }

            return sort switch
            {
                "date_time" => x => x.DateTime,
                "name" => x => x.Name,
                "price" => x => x.Price,
                "tickets_cnt" => x => x.TicketsQuantity,
                "booked_cnt" => x => x.Tickets.Count(t => !t.IsDeleted),
                "latest" => x => x.DateCreated,
                _ => x => x.DateTime
            };
        }

        #endregion

        #region Methods

        public async Task<DataResponse<PagedList<EventDto>>> GetEventsPaginated(int page, int pageSize, bool includeThumbnailImg = false)
        {
            PagedList<EventDto> pagedList = await _eventRepo.FindPaginatedAsync(
                predicate: x => !x.IsDeleted,
                pageNumber: page,
                pageSize: pageSize,
                mapFunc: e => new EventDto(e),
                include: x => x.Include(x => x.Tags).Include(x => x.Tickets),
                orderBy: x => x.DateTime,
                orderByDescending: false
            );

            if (includeThumbnailImg)
            {
                foreach (var eventDto in pagedList.Items)
                {
                    var img = await _imageService.GetEventThumbnailImg(eventDto.Id);

                    if (img != null)
                        eventDto.Image = new FileUploadDto(img);
                }
            }

            return Success(pagedList);
        }

        public async Task<DataResponse<PagedList<EventDto>>> GetFilteredEventsPaginated(PaginationModel pagination, EventFilterDto filterDto, bool includeThumbnailImg = false)
        {
            Expression<Func<EventEntity, bool>> filterPredicate = x => filterDto.GetDeletedEvents || !x.IsDeleted
                && (string.IsNullOrEmpty(filterDto.Search) || x.Name.ToLower().Contains(filterDto.Search.ToLower()))
                && (string.IsNullOrEmpty(filterDto.Category) || x.Category.ToLower() == filterDto.Category.ToLower())
                && (filterDto.TagIds.Count() == 0 || x.Tags.Any(t => filterDto.TagIds.Contains(t.Id)))
                && (filterDto.FromDate == null || x.DateTime.Date >= filterDto.FromDate.Value.ToDateTime(TimeOnly.MinValue))
                && (filterDto.ToDate == null || x.DateTime.Date <= filterDto.ToDate.Value.ToDateTime(TimeOnly.MinValue))
                && (!filterDto.UpcomingOnly || x.DateTime.Date > DateTime.Now.Date);

            PagedList<EventDto> pagedList = await _eventRepo.FindPaginatedAsync(
                predicate: filterPredicate,
                pageNumber: pagination.Page,
                pageSize: pagination.PageSize,
                mapFunc: e => new EventDto(e),
                include: x => x.Include(x => x.Tags),
                orderBy: GenerateSortingExpression(pagination.Sort),
                orderByDescending: pagination.IsOrderDesc()
            );

            foreach (var eventDto in pagedList.Items)
            {
                eventDto.BookedTickets = await _ticketRepo.CountAsync(x => x.EventId == eventDto.Id && !x.IsDeleted);

                var userId = _workContext.GetCurrentUserId();
                if (string.IsNullOrEmpty(userId)) continue;

                eventDto.IsBooked = await _ticketRepo
                    .Exists(x => x.EventId == eventDto.Id && x.UserId.ToString() == userId && !x.IsDeleted);

                if (!includeThumbnailImg) continue;

                var img = await _imageService.GetEventThumbnailImg(eventDto.Id);

                if (img != null)
                    eventDto.Image = new FileUploadDto(img);
            }

            return Success(pagedList);
        }

        public async Task<DataResponse<EventDetailsDto>> GetEventDetails(Guid eventId)
        {
            var @event = await _eventRepo.FirstOrDefaultAsync(
                predicate: e => e.Id == eventId,
                include: e => e.Include(x => x.Images).Include(x => x.Tags)
            );

            if (@event == null)
                return NotFound<EventDetailsDto>();

            var eventDto = new EventDetailsDto(@event);

            eventDto.BookedTickets = await _ticketRepo.CountAsync(x => x.EventId == eventDto.Id && !x.IsDeleted);

            var userId = _workContext.GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                eventDto.IsBooked = await _ticketRepo
                .Exists(x => x.EventId == eventDto.Id && x.UserId.ToString() == userId && !x.IsDeleted);
            }

            return Success(eventDto);
        }

        public async Task<DataResponse<Guid>> CreateEvent(CreateEventDto createDto)
        {
            var validationResult = _validatorDispatcher.Validate(createDto);
            if (!validationResult.IsValid)
                return ValidationError<Guid>(validationResult.Errors);

            await _unitOfWork.BeginTransactionAsync();

            IList<TagEntity> eventTags = [];
            if (createDto.Tags != null)
            {
                eventTags = await _tagService.ProcessTagsAsStrings(
                    tags: createDto.Tags,
                    returnOnlyNewTags: false,
                    persistNewTags: true
                );
            }

            var @event = new EventEntity
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Venue = createDto.Venue,
                Country = createDto.Country,
                City = createDto.City,
                Price = createDto.Price,
                Category = createDto.Category,
                DateTime = createDto.DateTime,
                TicketsQuantity = createDto.TicketsQty,
                Tags = eventTags
            };

            await _eventRepo.AddAsync(@event);
            await _unitOfWork.CommitAsync();

            await _imageService.SaveEventImages(createDto.Images, @event.Id, createDto.ThumbnailKey);
            await _unitOfWork.SaveChangesAsync();

            return Created(@event.Id);
        }

        public async Task<DataResponse<Guid>> EditEvent(EditEventDto editDto)
        {
            var @event = await _eventRepo.FirstOrDefaultAsync(
                predicate: x => x.Id == editDto.Id,
                include: x => x.Include(x => x.Tags));

            if (@event == null) return NotFound<Guid>();
            if (@event.IsDeleted) return BadRequest<Guid>("Deleted event cannot be updated.");
            if (@event.DateTime <= DateTime.Now) return BadRequest<Guid>("Already started or completed event cannot be updated.");

            var validationResult = _validatorDispatcher.Validate(editDto as CreateEventDto);
            if (@event.Tickets.Count > editDto.TicketsQty)
                validationResult.AddError(nameof(@event.TicketsQuantity), "Tickets quantity cannot be less than already booked tickets.");
            if (!validationResult.IsValid) return ValidationError<Guid>(validationResult.Errors);

            await _unitOfWork.BeginTransactionAsync();

            IList<TagEntity> eventTags = [];
            if (editDto.Tags != null)
            {
                eventTags = await _tagService.ProcessTagsAsStrings(
                    tags: editDto.Tags,
                    returnOnlyNewTags: false,
                    persistNewTags: true
                );
            }

            // add tags
            foreach (var tag in eventTags)
            {
                if (@event.Tags.Any(t => t.Id == tag.Id)) continue;
                @event.Tags.Add(tag);
            }
            // remove not selected tags
            foreach (var tag in @event.Tags)
            {
                if (!eventTags.Any(t => t.Id == tag.Id))
                {
                    @event.Tags.Remove(tag);
                }
            }

            @event.Name = editDto.Name;
            @event.Description = editDto.Description;
            @event.Venue = editDto.Venue;
            @event.Country = editDto.Country;
            @event.City = editDto.City;
            @event.Price = editDto.Price;
            @event.Category = editDto.Category;
            @event.DateTime = editDto.DateTime;
            @event.TicketsQuantity = editDto.TicketsQty;
            @event.Tags = eventTags;
            @event.DateTime = editDto.DateTime;

            _eventRepo.Update(@event);
            await _unitOfWork.CommitAsync();

            await _imageService.DeleteEventImages(editDto.DeletedImgIds);

            if (Guid.TryParse(editDto.ThumbnailKey, out Guid thumbnailImgId))
                await _imageService.SetImageAsThumbnail(thumbnailImgId);

            await _imageService.SaveEventImages(editDto.Images, @event.Id, editDto.ThumbnailKey);
            await _unitOfWork.SaveChangesAsync();

            return Success(@event.Id);
        }

        public async Task<BaseResponse> CancelEvent(Guid EventId)
        {
            var @event = await _eventRepo.FirstOrDefaultAsync(
                predicate: x => x.Id == EventId,
                include: x => x.Include(e => e.Tickets)
            );

            if (@event == null)
                return NotFound();

            if (@event.IsDeleted)
                return BadRequest("Event already deleted");

            if (@event.DateTime <= DateTime.Now)
                return BadRequest("Event already started or completed");

            await _unitOfWork.BeginTransactionAsync();

            @event.IsDeleted = true;
            foreach (var ticket in @event.Tickets)
                ticket.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return Success("Event cancelled successfully");
        }

        public async Task<DataResponse<IList<string>>> GetCategories(bool inUseOnly = true)
        {
            IList<string> categories = await _eventRepo.Table().Where(x => !inUseOnly || !x.IsDeleted)
                .Select(x => x.Category).Distinct().ToListAsync();

            return Success(categories);
        }

        #endregion
    }
}
