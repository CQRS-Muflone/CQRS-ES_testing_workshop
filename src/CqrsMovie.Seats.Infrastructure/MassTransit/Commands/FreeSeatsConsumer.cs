using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Seats.Domain.CommandHandlers;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace CqrsMovie.Seats.Infrastructure.MassTransit.Commands
{
    public class FreeSeatsConsumer : CommandConsumer<FreeSeats>
    {
        public FreeSeatsConsumer(IRepository repository, ILoggerFactory loggerFactory) : base(repository, loggerFactory)
        {
        }

        protected override ICommandHandler<FreeSeats> Handler => new FreeSeatsCommandHandler(Repository, LoggerFactory);
        public override async Task Consume(ConsumeContext<FreeSeats> context)
        {
            using var handler = this.Handler;
            await handler.Handle(context.Message);
        }
    }
}