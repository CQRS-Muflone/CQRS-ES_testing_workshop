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
        ISagaEventHandler<SeatsAlreadyTaken>
    {
        private static readonly Guid DailyProgramming1 = new Guid("ABD6E805-3C9D-4BE4-9B3F-FB8E22CC9D4A");

        public class SagaBookedState
        {
            public bool PaymentApproved;
            public bool SeatsBooked;
        }

        public BookSeatsSaga(IServiceBus serviceBus, ISagaRepository repository) : base(serviceBus, repository)
        {
        }

        public override async Task StartedBy(StartBookSeatsSaga command)
        {
            var sagaState = new SagaBookedState
            {
                PaymentApproved = false,
                SeatsBooked = false
            };
            await Repository.Save(command.Headers.CorrelationId, sagaState);

            await ServiceBus.Send(new BookSeats(new DailyProgrammingId(DailyProgramming1), command.Seats));
        }

        public Task Handle(SeatsBooked @event)
        {
            return Task.CompletedTask;
        }

        public Task Handle(SeatsAlreadyTaken @event)
        {
            return Task.CompletedTask;
        }
    }
}