using System.Threading.Tasks;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.SharedKernel.ReadModel;
using CqrsMovie.Website.ReadModel.EventHandlers;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;

namespace CqrsMovie.Website.Infrastructure.MassTransit.Events
{
  public class SeatsReservedConsumer : DomainEventConsumer<SeatsReserved>
  {
    public SeatsReservedConsumer(IPersister persister, ILoggerFactory loggerFactory) : base(persister, loggerFactory)
    {
    }

    protected override IDomainEventHandler<SeatsReserved> Handler => new SeatsReservedEventHandler(Persister, LoggerFactory);
    public override async Task Consume(ConsumeContext<SeatsReserved> context)
    {
      using (var handler = Handler)
        await handler.Handle(context.Message);
    }
  }
}