using System;
using System.Threading.Tasks;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Seats.Domain.Entities;
using CqrsMovie.SharedKernel.Domain.Ids;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace CqrsMovie.Seats.Domain.CommandHandlers
{
  public class CreateDailyProgrammingCommandHandler : CommandHandler<CreateDailyProgramming>
  {
    public CreateDailyProgrammingCommandHandler(IRepository repository, ILoggerFactory loggerFactory)
      : base(repository, loggerFactory)
    {
    }

    public override async Task Handle(CreateDailyProgramming command)
    {
      try
      {
          var entity = DailyProgramming.CreateDailyProgramming((DailyProgrammingId)command.AggregateId, command.MovieId, command.ScreenId, command.Date, command.Seats, command.MovieTitle, command.ScreenName);
          await Repository.Save(entity, Guid.NewGuid(), headers => { });
      }
      catch (Exception e)
      {
        Logger.LogError($"CreateDailyProgrammingCommand: Error processing the command: {e.Message} - StackTrace: {e.StackTrace}");
        throw;
      }
    }
  }
}
