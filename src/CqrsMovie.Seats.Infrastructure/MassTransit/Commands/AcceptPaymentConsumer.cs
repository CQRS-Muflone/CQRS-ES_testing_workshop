using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Payment;
using CqrsMovie.Seats.Domain.CommandHandlers;
using MassTransit;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace CqrsMovie.Seats.Infrastructure.MassTransit.Commands
{
    public class AcceptPaymentConsumer : CommandConsumer<AcceptPayment>
    {
        public AcceptPaymentConsumer(IRepository repository, ILoggerFactory loggerFactory) : base(repository, loggerFactory)
        {
        }

        protected override ICommandHandler<AcceptPayment> Handler =>
            new AcceptPaymentCommandHandler(Repository, this.LoggerFactory);
        public override async Task Consume(ConsumeContext<AcceptPayment> context)
        {
            using var handler = this.Handler;
            await handler.Handle(context.Message);
        }
    }
}