using Evenda.App.Contracts.IPersistence.IRepositories;
using Evenda.App.Contracts.IPersistence.IUnitOfWork;
using Evenda.App.Contracts.IServices.IEvent;
using Evenda.App.Dtos.Event;
using Evenda.App.Models;
using Evenda.App.Utils;
using Evenda.Services.Services.Base;
using Microsoft.EntityFrameworkCore;
using EventEntity = Evenda.Domain.Entities.EventEntities.Event;

namespace Evenda.App.Services.Event
{
    public class EventService : BaseService, IEventService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<EventEntity> _eventRepo;

        #endregion

        #region Ctor

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _eventRepo = _unitOfWork.GetRepository<EventEntity>();
        }

        #endregion

        #region Methods

        public async Task<DataResponse<PagedList<EventDto>>> GetEventsPaginated(int page, int pageSize)
        {
            PagedList<EventDto> pagedList = await _eventRepo.FindPaginatedAsync(
                predicate: x => !x.IsDeleted,
                pageNumber: page,
                pageSize: pageSize,
                mapFunc: e => new EventDto(e),
                include: x => x.Include(x => x.Images),
                orderBy: x => x.DateTime,
                orderByDescending: false
            );

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

        #endregion
    }
}
