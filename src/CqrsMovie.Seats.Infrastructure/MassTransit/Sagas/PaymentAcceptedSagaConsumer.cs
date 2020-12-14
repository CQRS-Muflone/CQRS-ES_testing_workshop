using System.Threading.Tasks;
using CqrsMovie.Messages.Events.Seat;
using MassTransit;
using Muflone;
using Muflone.MassTransit.RabbitMQ.Consumers;
using Muflone.Saga;
using Muflone.Saga.Persistence;

namespace CqrsMovie.Seats.Infrastructure.MassTransit.Sagas
{
    public class PaymentAcceptedSagaConsumer : SagaEventConsumerBase<PaymentAccepted>
    {
        private readonly ISagaRepository repository;
        private readonly IServiceBus serviceBus;

        public PaymentAcceptedSagaConsumer(ISagaRepository repository, IServiceBus serviceBus)
        {
            this.repository = repository;
            this.serviceBus = serviceBus;
        }

        protected override ISagaEventHandler<PaymentAccepted> Handler => new BookSeatsSaga(this.serviceBus, this.repository);
        public override async Task Consume(ConsumeContext<PaymentAccepted> context)
        {
            using (var handle = this.Handler)
            {
                await handle.Handle(context.Message);
            }
        }
    }
}