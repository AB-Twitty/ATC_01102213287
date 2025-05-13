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
        private readonly ITagService _tagService;
        private readonly IImageService _imageService;

        #endregion

        #region Ctor

        public EventService(IUnitOfWork unitOfWork, ITagService tagService, IImageService imageService, IValidatorDispatcher validatorDispatcher)
        {
            _unitOfWork = unitOfWork;
            _eventRepo = _unitOfWork.GetRepository<EventEntity>();
            _imageRepo = _unitOfWork.GetRepository<Image>();
            _tagService = tagService;
            _imageService = imageService;
            _validatorDispatcher = validatorDispatcher;
        }

        #endregion

        #region Utils

        protected virtual Expression<Func<EventEntity, object>> GenerateSortingExpression(string? sort)
        {
            var allowedSorts = new[] { "date_time", "name", "price", "#tickets", "#booked" };

            if (string.IsNullOrWhiteSpace(sort) || !allowedSorts.Contains(sort))
            {
                sort = allowedSorts[0];
            }

            return sort switch
            {
                "date_time" => x => x.DateTime,
                "name" => x => x.Name,
                "price" => x => x.Price,
                "#tickets" => x => x.TicketsQuantity,
                "#booked" => x => x.TicketsQuantity,
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
                include: x => x.Include(x => x.Tags),
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
            Expression<Func<EventEntity, bool>> filterPredicate = x => !x.IsDeleted
                && (string.IsNullOrEmpty(filterDto.Search) || x.Name.ToLower().Contains(filterDto.Search.ToLower()))
                && (string.IsNullOrEmpty(filterDto.Category) || x.Name.ToLower() == filterDto.Category.ToLower())
                && (filterDto.TagIds.Count() == 0 || x.Tags.Any(t => filterDto.TagIds.Contains(t.Id)))
                && (filterDto.FromDate == null || x.DateTime.Date >= filterDto.FromDate.Value.ToDateTime(TimeOnly.MinValue))
                && (filterDto.ToDate == null || x.DateTime.Date <= filterDto.ToDate.Value.ToDateTime(TimeOnly.MinValue));

            PagedList<EventDto> pagedList = await _eventRepo.FindPaginatedAsync(
                predicate: filterPredicate,
                pageNumber: pagination.Page,
                pageSize: pagination.PageSize,
                mapFunc: e => new EventDto(e),
                include: x => x.Include(x => x.Tags),
                orderBy: GenerateSortingExpression(pagination.Sort),
                orderByDescending: pagination.IsOrderDesc()
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

        public async Task<DataResponse<EventDetailsDto>> GetEventDetails(Guid eventId)
        {
            var @event = await _eventRepo.FirstOrDefaultAsync(
                predicate: e => e.Id == eventId,
                include: e => e.Include(x => x.Images)
            );

            if (@event == null)
                return NotFound<EventDetailsDto>();

            return Success(new EventDetailsDto(@event));
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

            await _imageService.SaveEventImages(createDto.Images, @event.Id, createDto.ThumbnailIdx);

            return Created(@event.Id);
        }

        #endregion
    }
}
