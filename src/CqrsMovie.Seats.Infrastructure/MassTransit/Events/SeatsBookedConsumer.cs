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

        //protected override IDomainEventHandler<SeatsBooked> Handler => new SeatsBookedDomainEventHandler(Persister, LoggerFactory);
        protected override IDomainEventHandler<SeatsBooked> Handler => new DailyProgrammingSaga(this.serviceBus, this.seatsService);
        public override async Task Consume(ConsumeContext<SeatsBooked> context)
        {
            using var handler = this.Handler;
            await handler.Handle(context.Message);
        }
    }
}