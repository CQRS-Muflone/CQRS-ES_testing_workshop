using System;
using System.Collections.Generic;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Messages.Events;

namespace CqrsMovie.Messages.Events.Seat
{
  public class DailyProgrammingCreated : DomainEvent
  {
    public MovieId MovieId { get; }
    public ScreenId ScreenId { get; }
    public DateTime Date { get; }
    public IEnumerable<Dtos.Seat> Seats { get; }
    public string MovieTitle { get; }
    public string ScreenName { get; }

    public DailyProgrammingCreated(DailyProgrammingId aggregateId, MovieId movieId, ScreenId screenId, DateTime date, IEnumerable<Dtos.Seat> seats, string movieTitle, string screenName, string who = "anonymous")
      : base(aggregateId, who)
    {
      MovieId = movieId;
      ScreenId = screenId;
      Date = date;
      Seats = seats;
      MovieTitle = movieTitle;
      ScreenName = screenName;
    }
  }
}
