using System;
using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone;
using Muflone.Saga;
using Muflone.Saga.Persistence;

namespace CqrsMovie.Seats.Infrastructure.MassTransit.Sagas
{
    public class BookSeatsSaga : SagaStartedByHandler<StartBookSeatsSaga, BookSeatsSaga.SagaBookedState>,
        ISagaEventHandler<SeatsBooked>,
        ISagaEventHandler<PaymentAccepted>,
        ISagaEventHandler<SeatsAlreadyTaken>
    {
        private static readonly Guid DailyProgramming1 = new Guid("ABD6E805-3C9D-4BE4-9B3F-FB8E22CC9D4A");

        public class SagaBookedState
        {
            public bool PaymentApproved;
            public bool SeatsBooked;
            public bool SeatsUnReserved;
        }

        public BookSeatsSaga(IServiceBus serviceBus, ISagaRepository repository) : base(serviceBus, repository)
        {
        }

        public override async Task StartedBy(StartBookSeatsSaga command)
        {
            var sagaState = new SagaBookedState
            {
                PaymentApproved = false,
                SeatsBooked = false,
                SeatsUnReserved = false
            };
            await Repository.Save(command.Headers.CorrelationId, sagaState);

            await ServiceBus.Send(new BookSeats(new DailyProgrammingId(DailyProgramming1), command.Seats));
        }

        public async Task Handle(SeatsBooked @event)
        {
            try
            {
                await this.ServiceBus.Send(new RequestPayment(new PaymentId(Guid.NewGuid()), Guid.NewGuid()));

                var sagaState = await this.Repository.GetById<SagaBookedState>(@event.Headers.CorrelationId);
                sagaState.SeatsBooked = true;
                await Repository.Save(@event.Headers.CorrelationId, sagaState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task Handle(SeatsAlreadyTaken @event)
        {
            await this.ServiceBus.Send(new UnreserveSeats((DailyProgrammingId) @event.AggregateId, @event.Seats));
        }

        public Task Handle(PaymentAccepted @event)
        {
            // Send email to our Customer
            return Task.CompletedTask;
        }
    }
}