using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Seats.Domain.CommandHandlers;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace CqrsMovie.Seats.Infrastructure.MassTransit.Commands
{
  public class ReserveSeatsConsumer : CommandConsumer<ReserveSeats>
  {
    public ReserveSeatsConsumer(IRepository repository, ILoggerFactory loggerFactory) : base(repository, loggerFactory)
    {
    }

    protected override ICommandHandler<ReserveSeats> Handler => new ReserveSeatsCommandHandler(Repository, LoggerFactory);
    public override async Task Consume(ConsumeContext<ReserveSeats> context)
    {
      using (var handler = Handler)
        await handler.Handle(context.Message);
    }
  }
}