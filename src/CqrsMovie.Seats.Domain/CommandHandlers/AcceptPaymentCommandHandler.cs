using System;
using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Payment;
using CqrsMovie.Seats.Domain.Entities;
using CqrsMovie.SharedKernel.Domain.Ids;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace CqrsMovie.Seats.Domain.CommandHandlers
{
    public class AcceptPaymentCommandHandler : CommandHandler<AcceptPayment>
    {
        public AcceptPaymentCommandHandler(IRepository repository, ILoggerFactory loggerFactory) : base(repository, loggerFactory)
        {
        }

        public override async Task Handle(AcceptPayment command)
        {
            try
            {
                var entity = await Repository.GetById<DailyProgramming>(command.AggregateId);
                entity.AcceptPayment((DailyProgrammingId)entity.Id, command.Headers.CorrelationId);
                await Repository.Save(entity, Guid.NewGuid(), headers => { });
            }
            catch (Exception e)
            {
                Logger.LogError($"AcceptPaymentCommandHandler: Error processing the command: {e.Message} - StackTrace: {e.StackTrace}");
                throw;
            }
        }
    }
}