using System.Threading.Tasks;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.Seats.ReadModel.Services.Abstracts;
using CqrsMovie.SharedKernel.ReadModel;
using Microsoft.Extensions.Logging;

namespace CqrsMovie.Seats.ReadModel.EventHandlers
{
    public class DailyProgrammingCreatedDomainDomainEventHandler : DomainEventHandler<DailyProgrammingCreated>
    {
        private readonly ISeatsService seatsService;

        public DailyProgrammingCreatedDomainDomainEventHandler(IPersister persister, ILoggerFactory loggerFactory, ISeatsService seatsService)
          : base(persister, loggerFactory)
        {
            this.seatsService = seatsService;
        }

        public override async Task Handle(DailyProgrammingCreated @event)
        {
            await this.seatsService.AddDailyProgrammingAsync(@event);
        }
    }
}