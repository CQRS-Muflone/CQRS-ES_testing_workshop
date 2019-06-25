using Microsoft.Extensions.Logging;
using Muflone.MassTransit.RabbitMQ.Consumers;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace CqrsMovie.Seats.Infrastructure.MassTransit.Commands
{
  public abstract class CommandConsumer<TCommand> : CommandConsumerBase<TCommand> where TCommand : Command
  {
    protected readonly IRepository Repository;
    protected readonly ILoggerFactory LoggerFactory;

    protected CommandConsumer(IRepository repository, ILoggerFactory loggerFactory)
    {
      Repository = repository;
      LoggerFactory = loggerFactory;
    }
  }
}