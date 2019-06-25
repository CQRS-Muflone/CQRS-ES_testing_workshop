using System.Threading.Tasks;
using CqrsMovie.Core.Enums;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.SharedKernel.ReadModel;
using CqrsMovie.Website.ReadModel.Dtos;
using Microsoft.Extensions.Logging;

namespace CqrsMovie.Website.ReadModel.EventHandlers
{
  public class DailyProgrammingCreatedDomainDomainEventHandler : DomainEventHandler<DailyProgrammingCreated>
  {
    public DailyProgrammingCreatedDomainDomainEventHandler(IPersister persister, ILoggerFactory loggerFactory)
      : base(persister, loggerFactory)
    {
    }

    public override async Task Handle(DailyProgrammingCreated @event)
    {
      var entity = new DailyProgramming()
      {
        Date = @event.Date,
        Id = @event.AggregateId.ToString(),
        ScreenId = @event.ScreenId.ToString(),
        Seats = @event.Seats.ToReadModel(SeatState.Free),
        MovieId = @event.MovieId.ToString(),
        MovieTitle = @event.MovieTitle,
        ScreenName = @event.ScreenName
      };
      await Persister.Insert(entity);
    }
  }
}