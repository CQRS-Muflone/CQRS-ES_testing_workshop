using System.Threading.Tasks;
using CqrsMovie.Messages.Events.Seat;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Saga;
using Muflone.Saga.Persistence;

namespace CqrsMovie.Sagas.Infrastructure.MassTransit
{
	public class SeatsAlreadyTakenSagaConsumer : SagaEventConsumer<SeatsAlreadyTaken>
	{
		public SeatsAlreadyTakenSagaConsumer(ISagaRepository repository, IServiceBus serviceBus, ILoggerFactory loggerFactory)
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