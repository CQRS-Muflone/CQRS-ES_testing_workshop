using System;
using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Seats.Domain.Entities;
using CqrsMovie.SharedKernel.Domain.Ids;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace CqrsMovie.Seats.Domain.CommandHandlers
{
    public class UnreserveSeatsCommandHandler : CommandHandler<UnreserveSeats>
    {
        public UnreserveSeatsCommandHandler(IRepository repository, ILoggerFactory loggerFactory) : base(repository, loggerFactory)
        {
        }

        public override async Task Handle(UnreserveSeats command)
        {
            try
            {
                var entity = await Repository.GetById<DailyProgramming>(command.AggregateId);
                entity.UnReserveSeat((DailyProgrammingId)entity.Id, command.Seats);
                await Repository.Save(entity, Guid.NewGuid(), headers => { });
            }
            catch (Exception e)
            {
                Logger.LogError($"UnReserveSeatsCommand: Error processing the command: {e.Message} - StackTrace: {e.StackTrace}");
                throw;
            }
        }
    }
}