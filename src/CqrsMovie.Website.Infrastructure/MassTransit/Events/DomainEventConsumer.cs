using CqrsMovie.SharedKernel.ReadModel;
using Microsoft.Extensions.Logging;
using Muflone.MassTransit.RabbitMQ.Consumers;
using Muflone.Messages.Events;

namespace CqrsMovie.Website.Infrastructure.MassTransit.Events
{
  public abstract class DomainEventConsumer<TEvent> : DomainEventConsumerBase<TEvent> where TEvent : DomainEvent
  {
    protected readonly IPersister Persister;
    protected readonly ILoggerFactory LoggerFactory;

    protected DomainEventConsumer(IPersister persister, ILoggerFactory loggerFactory)
    {
      Persister = persister;
      LoggerFactory = loggerFactory;
    }
  }
}