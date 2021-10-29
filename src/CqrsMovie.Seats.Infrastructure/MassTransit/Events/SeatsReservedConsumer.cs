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
    public class SeatsReservedConsumer : DomainEventConsumer<SeatsReserved>
    {
        private readonly IServiceBus serviceBus;
        private readonly ISeatsService seatsService;

        public SeatsReservedConsumer(IPersister persister, ILoggerFactory loggerFactory,
            IServiceBus serviceBus, ISeatsService seatsService) : base(persister, loggerFactory)
        {
            this.serviceBus = serviceBus;
            this.seatsService = seatsService;
        }

        //protected override IDomainEventHandler<SeatsReserved> Handler => new SeatsReservedDomainEventHandler(Persister, LoggerFactory);
        protected override IDomainEventHandler<SeatsReserved> Handler => new StartSagaFromReserveSeat(this.serviceBus, this.seatsService);
        public override async Task Consume(ConsumeContext<SeatsReserved> context)
        {
            using var handler = this.Handler;
            await handler.Handle(context.Message);
        }
    }
}