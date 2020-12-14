using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Saga;
using Muflone.Saga.Persistence;

namespace CqrsMovie.Sagas.Infrastructure.MassTransit
{
	public class StartBookSeatsSagaConsumer : SagaStartedByConsumer<StartBookSeatsSaga>
	{
		public StartBookSeatsSagaConsumer(ISagaRepository repository, IServiceBus serviceBus, ILoggerFactory loggerFactory)
				: base(repository, serviceBus, loggerFactory)
		{
		}

		protected override ISagaStartedBy<StartBookSeatsSaga> Handler => new BookSeatsSaga(this.ServiceBus, this.Repository);
		public override async Task Consume(ConsumeContext<StartBookSeatsSaga> context)
		{
			using (var handler = this.Handler)
				await handler.StartedBy(context.Message);
		}
	}
}