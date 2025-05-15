using Evenda.App.Contracts;
using Evenda.App.Contracts.IPersistence.IRepositories;
using Evenda.App.Contracts.IPersistence.IUnitOfWork;
using Evenda.App.Contracts.IServices.ITickets;
using Evenda.App.Dtos.Tickets;
using Evenda.App.Models;
using Evenda.Domain.Entities.TicketEntities;
using Evenda.Services.Services.Base;

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

        #endregion
    }
}
