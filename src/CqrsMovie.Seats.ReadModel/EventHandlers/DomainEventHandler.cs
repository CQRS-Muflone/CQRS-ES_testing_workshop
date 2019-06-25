using System;
using System.Threading.Tasks;
using CqrsMovie.SharedKernel.ReadModel;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;

namespace CqrsMovie.Seats.ReadModel.EventHandlers
{
  public abstract class DomainEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IDomainEvent
  {
    protected readonly IPersister Persister;
    protected readonly ILoggerFactory LoggerFactory;

    protected DomainEventHandler(IPersister persister, ILoggerFactory loggerFactory)
    {
      Persister = persister;
      LoggerFactory = loggerFactory;
    }

    public abstract Task Handle(TEvent command);

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

    ~DomainEventHandler()
    {
      Dispose(false);
    }
  }
}