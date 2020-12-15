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
        private static readonly Guid SagaCorrelationId = new Guid("ABD6E805-3C9D-4BE4-9B3F-FB8E22CC9D4A");

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
            await Repository.Save(SagaCorrelationId, sagaState);

            await this.ServiceBus.Send(new RequestPayment(new PaymentId(Guid.NewGuid()), SagaCorrelationId,
                command.Seats));
        }

        public async Task Handle(PaymentAccepted @event)
        {
            var sagaState = await this.GetById(SagaCorrelationId);
            sagaState.PaymentApproved = true;
            sagaState.SeatsBooked = true;
            await Repository.Save(SagaCorrelationId, sagaState);
            
            await ServiceBus.Send(new BookSeats(new DailyProgrammingId(DailyProgramming1), @event.Seats));
        }

        public async Task Handle(SeatsBooked @event)
        {
            try
            {
                //var sagaState = await this.Repository.GetById<SagaBookedState>(SagaCorrelationId);
                var sagaState = await this.GetById(SagaCorrelationId);
                sagaState.PaymentApproved = true;
                await Repository.Save(SagaCorrelationId, sagaState);
                
                // Send Email to customer
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public Task Handle(SeatsAlreadyTaken @event)
        {
            // Send Email to customer
            return Task.CompletedTask;
        }

        private async Task<SagaBookedState> GetById(Guid correlationId)
        {
            var sagaState = new SagaBookedState
            {
                PaymentApproved = false,
                SeatsBooked = false,
                SeatsUnReserved = false
            };

            return await Task.FromResult(sagaState);
        }
    }
}