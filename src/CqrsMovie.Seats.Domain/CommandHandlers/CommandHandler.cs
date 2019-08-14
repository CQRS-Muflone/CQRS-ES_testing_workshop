using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Muflone.Core;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace CqrsMovie.Seats.Domain.CommandHandlers
{
  public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
  {
    protected readonly IRepository Repository;
    protected readonly ILogger Logger;

    protected CommandHandler(IRepository repository, ILoggerFactory loggerFactory)
    {
      Repository = repository ?? throw new ArgumentNullException(nameof(repository));
      Logger = loggerFactory.CreateLogger(GetType());
    }

    protected async Task<TAggregate> Get<TAggregate>(IDomainId id) where TAggregate : AggregateRoot
    {
      var aggregate = await Repository.GetById<TAggregate>(id);
      if (aggregate == null)
        throw new Exception($"{typeof(TAggregate).Name} not found");
      return aggregate;
    }

    protected async Task Save<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot
    {
      await Repository.Save(aggregate, Guid.NewGuid());
    }

    public abstract Task Handle(TCommand command);

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

    ~CommandHandler()
    {
      Dispose(false);
    }
  }
}