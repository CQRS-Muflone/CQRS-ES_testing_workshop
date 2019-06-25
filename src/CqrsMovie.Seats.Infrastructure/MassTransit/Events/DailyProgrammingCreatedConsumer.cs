using System.Threading.Tasks;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.Seats.ReadModel.EventHandlers;
using CqrsMovie.SharedKernel.ReadModel;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;

namespace CqrsMovie.Seats.Infrastructure.MassTransit.Events
{
  public class DailyProgrammingCreatedConsumer : DomainEventConsumer<DailyProgrammingCreated>
  {
    public DailyProgrammingCreatedConsumer(IPersister persister, ILoggerFactory loggerFactory) : base(persister, loggerFactory)
    {
    }

    protected override IDomainEventHandler<DailyProgrammingCreated> Handler => new DailyProgrammingCreatedDomainDomainEventHandler(Persister, LoggerFactory);
    public override async Task Consume(ConsumeContext<DailyProgrammingCreated> context)
    {
      using (var handler = Handler)
        await handler.Handle(context.Message);
    }
  }
}
