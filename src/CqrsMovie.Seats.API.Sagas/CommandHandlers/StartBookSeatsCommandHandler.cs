using System;
using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.SharedKernel.Domain.Ids;
using Microsoft.Extensions.Logging;

namespace CqrsMovie.Seats.API.Sagas.CommandHandlers
{
  public class StartBookSeatsCommandHandler : CommandHandler<StartBookSeatsSaga>
  {
    public StartBookSeatsCommandHandler(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
    }

    public override async Task Handle(StartBookSeatsSaga command)
    {
      try
      {
          var hereWeAre = "Here we are";
          //entity.BookSeats((DailyProgrammingId)entity.Id, command.Seats);
      }
      catch (Exception e)
      {
        this.Logger.LogError($"BookSeatsCommand: Error processing the command: {e.Message} - StackTrace: {e.StackTrace}");
        throw;
      }
    }
  }
}