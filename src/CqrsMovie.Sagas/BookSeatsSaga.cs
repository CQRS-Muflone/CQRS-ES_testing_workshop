using System;
using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone;
using Muflone.Saga;
using Muflone.Saga.Persistence;

namespace CqrsMovie.Sagas
{
	public class BookSeatsSaga : ISagaStartedBy<StartBookSeatsSaga>, ISagaEventHandler<SeatsBooked>, ISagaEventHandler<SeatsAlreadyTaken>
	{
		private readonly IServiceBus serviceBus;
		private readonly ISagaRepository repository;
		
		public class SagaBookedState
		{
			public bool PaymentApproved;
			public bool SeatsBooked;
		}

		public BookSeatsSaga(IServiceBus serviceBus, ISagaRepository repository)
		{
			this.serviceBus = serviceBus;
			this.repository = repository;
		}

		public async Task StartedBy(StartBookSeatsSaga command)
		{
			var sagaState = new SagaBookedState
			{
				PaymentApproved = false,
				SeatsBooked = false
			};
			await repository.Save(command.Headers.CorrelationId, sagaState);

			await serviceBus.Send(new BookSeats((DailyProgrammingId)command.AggregateId, command.Seats));
		}

		public Task Handle(SeatsBooked @event)
		{
			return Task.CompletedTask;
		}

		public Task Handle(SeatsAlreadyTaken @event)
		{
			return Task.CompletedTask;
		}


		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~BookSeatsSaga()
		{
			Dispose(false);
		}
	}
}