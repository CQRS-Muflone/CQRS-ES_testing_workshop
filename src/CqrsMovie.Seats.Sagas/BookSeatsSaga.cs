using System;
using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone;
using Muflone.Saga;
using Muflone.Saga.Persistence;

namespace CqrsMovie.Seats.Sagas
{
    public class BookSeatsSaga : Saga<BookSeatsSaga.MyData>,
        ISagaStartedBy<StartBookSeatsSaga>,
        ISagaEventHandler<PaymentAccepted>
    {
        public class MyData
        {
            public string Value1;
            public string Value2;
        }

        public BookSeatsSaga(IServiceBus serviceBus, ISagaRepository repository) : base(serviceBus, repository)
        {
        }

        public async Task StartedBy(StartBookSeatsSaga command)
        {
            await this.ServiceBus.Send(new RequestPayment(new PaymentId(Guid.NewGuid()), Guid.NewGuid()));
        }

        public Task Handle(PaymentAccepted @event)
        {
            return Task.CompletedTask;
        }
    }
}