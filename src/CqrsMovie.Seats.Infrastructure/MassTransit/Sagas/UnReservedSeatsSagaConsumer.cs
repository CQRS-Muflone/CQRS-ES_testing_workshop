using System.Threading.Tasks;
using CqrsMovie.Messages.Events.Seat;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Saga;
using Muflone.Saga.Persistence;

namespace CqrsMovie.Seats.Infrastructure.MassTransit.Sagas
{
    public class UnReservedSeatsSagaConsumer : SagaEventConsumer<SeatsAlreadyTaken>
    {
        public UnReservedSeatsSagaConsumer(ISagaRepository repository, IServiceBus serviceBus, ILoggerFactory loggerFactory)
            : base(repository, serviceBus, loggerFactory)
        {
        }

        protected override ISagaEventHandler<SeatsAlreadyTaken> Handler => new BookSeatsSaga(this.ServiceBus, this.Repository);
        public override async Task Consume(ConsumeContext<SeatsAlreadyTaken> context)
        {
            using (var handle = Handler)
            {
                await handle.Handle(context.Message);
            }
        }
    }
}