using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Seats.Domain.CommandHandlers;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace CqrsMovie.Seats.Infrastructure.MassTransit.Commands
{
    public class RequestPaymentConsumer : CommandConsumer<RequestPayment>
    {
        public RequestPaymentConsumer(IRepository repository, ILoggerFactory loggerFactory) : base(repository, loggerFactory)
        {
        }

        protected override ICommandHandler<RequestPayment> Handler => new RequestPaymentCommandHandler(Repository, LoggerFactory);
        public override async Task Consume(ConsumeContext<RequestPayment> context)
        {
            using (var handler = Handler)
                await handler.Handle(context.Message);
        }
    }
}