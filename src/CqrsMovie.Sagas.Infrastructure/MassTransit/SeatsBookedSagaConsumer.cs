using System.Threading.Tasks;
using CqrsMovie.Messages.Events.Seat;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Saga;
using Muflone.Saga.Persistence;

namespace CqrsMovie.Sagas.Infrastructure.MassTransit
{
	public class SeatsBookedSagaConsumer : SagaEventConsumer<SeatsBooked>
	{
		public SeatsBookedSagaConsumer(ISagaRepository repository, IServiceBus serviceBus, ILoggerFactory loggerFactory) : base(repository, serviceBus, loggerFactory)
		{
		}

		protected override ISagaEventHandler<SeatsBooked> Handler => new BookSeatsSaga(this.ServiceBus, this.Repository);

		public override async Task Consume(ConsumeContext<SeatsBooked> context)
		{
			//if (context.CorrelationId != null)
			//{
			//	var sagaState = this.Repository.GetById<BookSeatsSaga.SagaBookedState>(context.CorrelationId.Value);
			//}

			using (var handler = this.Handler)
				await handler.Handle(context.Message);
		}
	}
}