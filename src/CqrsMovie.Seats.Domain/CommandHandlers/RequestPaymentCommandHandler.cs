using System;
using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Seats.Domain.Entities;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace CqrsMovie.Seats.Domain.CommandHandlers
{
    public sealed class RequestPaymentCommandHandler : CommandHandler<RequestPayment>
    {
        public RequestPaymentCommandHandler(IRepository repository, ILoggerFactory loggerFactory) : base(repository, loggerFactory)
        {
        }

        public override async Task Handle(RequestPayment command)
        {
            try
            {
                var aggregate = Payment.CreatePayment(command.PaymentId, command.Seats, command.Headers.CorrelationId);
                await Repository.Save(aggregate, Guid.NewGuid(), headers => { });
            }
            catch (Exception ex)
            {
                Logger.LogError($"RequestPayment: Error processing the command: {ex.Message} - StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}