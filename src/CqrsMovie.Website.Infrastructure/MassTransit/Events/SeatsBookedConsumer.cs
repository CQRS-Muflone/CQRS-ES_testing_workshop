using System.Threading.Tasks;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.SharedKernel.ReadModel;
using CqrsMovie.Website.ReadModel.EventHandlers;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;

namespace CqrsMovie.Website.Infrastructure.MassTransit.Events
{
  public class SeatsBookedConsumer : DomainEventConsumer<SeatsBooked>
  {
    public SeatsBookedConsumer(IPersister persister, ILoggerFactory loggerFactory) : base(persister, loggerFactory)
    {
    }

    protected override IDomainEventHandler<SeatsBooked> Handler => new SeatsBookedDomainEventHandler(Persister, LoggerFactory);
    public override async Task Consume(ConsumeContext<SeatsBooked> context)
    {
      using (var handler = Handler)
        await handler.Handle(context.Message);
    }
  }
}