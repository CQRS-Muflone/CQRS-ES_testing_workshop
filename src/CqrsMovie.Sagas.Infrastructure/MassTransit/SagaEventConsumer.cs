using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.MassTransit.RabbitMQ.Consumers;
using Muflone.Messages.Events;
using Muflone.Saga.Persistence;

namespace CqrsMovie.Sagas.Infrastructure.MassTransit
{
	public abstract class SagaEventConsumer<TEvent> : SagaEventConsumerBase<TEvent> where TEvent : Event
	{
		protected readonly ISagaRepository Repository;
		protected readonly IServiceBus ServiceBus;
		protected readonly ILoggerFactory LoggerFactory;
		protected readonly ILogger Logger;

		protected SagaEventConsumer(ISagaRepository repository, IServiceBus serviceBus, ILoggerFactory loggerFactory)
		{
			Repository = repository;
			ServiceBus = serviceBus;
			LoggerFactory = loggerFactory;
			Logger = loggerFactory.CreateLogger(GetType());
		}
	}
}