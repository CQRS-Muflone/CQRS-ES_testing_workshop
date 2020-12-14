using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.MassTransit.RabbitMQ.Consumers;
using Muflone.Messages.Commands;
using Muflone.Saga.Persistence;

namespace CqrsMovie.Sagas.Infrastructure.MassTransit
{
	public abstract class SagaStartedByConsumer<TCommand> : SagaStartedByConsumerBase<TCommand> where TCommand : Command
	{
		protected readonly ISagaRepository Repository;
		protected readonly IServiceBus ServiceBus;
		protected readonly ILoggerFactory LoggerFactory;
		protected readonly ILogger Logger;

		protected SagaStartedByConsumer(ISagaRepository repository, IServiceBus serviceBus, ILoggerFactory loggerFactory)
		{
			Repository = repository;
			ServiceBus = serviceBus;
			LoggerFactory = loggerFactory;
			Logger = loggerFactory.CreateLogger(GetType());
		}
	}
}