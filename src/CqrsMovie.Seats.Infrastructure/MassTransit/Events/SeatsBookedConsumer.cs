using System.Threading.Tasks;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.Seats.Domain.Sagas;
using CqrsMovie.Seats.ReadModel.Services.Abstracts;
using CqrsMovie.SharedKernel.ReadModel;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Messages.Events;

namespace CqrsMovie.Seats.Infrastructure.MassTransit.Events
{
    public class SeatsBookedConsumer : DomainEventConsumer<SeatsBooked>
    {
        private readonly IServiceBus serviceBus;
        private readonly ISeatsService seatsService;

        public SeatsBookedConsumer(IPersister persister, ILoggerFactory loggerFactory,
            IServiceBus serviceBus, ISeatsService seatsService) : base(persister, loggerFactory)
        {
            this.serviceBus = serviceBus;
            this.seatsService = seatsService;
        }

        // Use this without Saga
        //protected override IDomainEventHandler<SeatsBooked> Handler => new SeatsBookedDomainEventHandler(Persister, LoggerFactory);

        // Use this with Saga from ReserveSeats
        //protected override IDomainEventHandler<SeatsBooked> Handler => new StartSagaFromReserveSeat(this.serviceBus, this.seatsService);

        // Use this with Saga from SeatsReserved
        protected override IDomainEventHandler<SeatsBooked> Handler => new StartSagaFromSeatReserved(this.serviceBus, this.seatsService);

        public override async Task Consume(ConsumeContext<SeatsBooked> context)
        {
            using var handler = this.Handler;
            await handler.Handle(context.Message);
        }
    }
}