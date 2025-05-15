using Evenda.App.Contracts;
using Evenda.App.Contracts.IPersistence.IRepositories;
using Evenda.App.Contracts.IPersistence.IUnitOfWork;
using Evenda.App.Contracts.IServices.ITickets;
using Evenda.App.Dtos.Tickets;
using Evenda.App.Models;
using Evenda.App.Utils;
using Evenda.Domain.Entities.TicketEntities;
using Evenda.Services.Services.Base;
using Microsoft.EntityFrameworkCore;
using EventEntity = Evenda.Domain.Entities.EventEntities.Event;

namespace Evenda.App.Services.Tickets
{
    public class TicketService : BaseService, ITicketService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Ticket> _ticketRepo;
        private readonly IBaseRepository<EventEntity> _eventRepo;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public TicketService(IUnitOfWork unitOfWork, IWorkContext workContext)
        {
            _unitOfWork = unitOfWork;
            _ticketRepo = unitOfWork.GetRepository<Ticket>();
            _eventRepo = unitOfWork.GetRepository<EventEntity>();
            _workContext = workContext;
        }

        #endregion

        #region Features

        public async Task<DataResponse<Guid>> BookEvent(BookEventDto bookDto)
        {
            var @event = await _eventRepo.FirstOrDefaultAsync(
                predicate: e => e.Id == bookDto.EventId && !e.IsDeleted
            );

            if (@event is null)
                return NotFound<Guid>($"No event found with this Id ({bookDto.EventId}).");

            var userId = _workContext.GetCurrentUserId();

            if (await _ticketRepo.Exists(t => t.EventId == bookDto.EventId && t.UserId.ToString() == userId))
                return BadRequest<Guid>($"You already booked a ticket for this event.");

            if (@event.Tickets.Count >= @event.TicketsQuantity)
                return BadRequest<Guid>($"No tickets available for this event.");

            var ticket = new Ticket
            {
                Event = @event,
                UserId = Guid.Parse(userId)
            };

            await _ticketRepo.AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            return Created(ticket.Id, "Ticket booked successfully.");
        }

        public async Task<DataResponse<PagedList<TicketDto>>> GetUserBookings(PaginationModel pagination)
        {
            var userId = _workContext.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized<PagedList<TicketDto>>();

            var tickets = await _ticketRepo.FindPaginatedAsync(
                predicate: x => true,
                pageNumber: pagination.Page,
                pageSize: pagination.PageSize,
                mapFunc: x => new TicketDto(x),
                include: x => x.Include(t => t.Event),
                orderBy: x => x.DateCreated,
                orderByDescending: true
            );

            return Success(tickets);
        }

        #endregion
    }
}
