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
    public class SeatsAlreadyFreedConsumer : DomainEventConsumer<SeatsAlreadyFreed>
    {
        private readonly IServiceBus serviceBus;
        private readonly ISeatsService seatsService;

        public SeatsAlreadyFreedConsumer(IPersister persister, ILoggerFactory loggerFactory, IServiceBus serviceBus,
            ISeatsService seatsService) : base(persister, loggerFactory)
        {
            this.serviceBus = serviceBus;
            this.seatsService = seatsService;
        }

        // Use this with Saga from ReserveSeat
        //protected override IDomainEventHandler<SeatsAlreadyFreed> Handler => new StartSagaFromReserveSeat(serviceBus, seatsService);

        // Use this with Saga from SeatsReserved
        protected override IDomainEventHandler<SeatsAlreadyFreed> Handler => new StartSagaFromSeatReserved(serviceBus, seatsService);

        public override async Task Consume(ConsumeContext<SeatsAlreadyFreed> context)
        {
            using var handler = this.Handler;
            await handler.Handle(context.Message);
        }
    }
}