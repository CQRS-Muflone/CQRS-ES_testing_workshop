using System;
using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Seats.Domain.Entities;
using CqrsMovie.SharedKernel.Domain.Ids;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace CqrsMovie.Seats.Domain.CommandHandlers
{
    public class BookSeatsCommandHandler : CommandHandler<BookSeats>
    {
        public BookSeatsCommandHandler(IRepository repository, ILoggerFactory loggerFactory) : base(repository, loggerFactory)
        {
        }

        public override async Task Handle(BookSeats command)
        {
            try
            {
                var entity = await Repository.GetById<DailyProgramming>(command.AggregateId);
                entity.BookSeats((DailyProgrammingId)entity.Id, command.CommitId, command.Seats);
                await Repository.Save(entity, Guid.NewGuid(), headers => { });
            }
            catch (Exception e)
            {
                Logger.LogError($"BookSeatsCommand: Error processing the command: {e.Message} - StackTrace: {e.StackTrace}");
                throw;
            }
        }
    }
}