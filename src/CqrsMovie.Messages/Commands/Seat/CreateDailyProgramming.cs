using System;
using System.Collections.Generic;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Messages.Commands;

namespace CqrsMovie.Messages.Commands.Seat
{
  public class CreateDailyProgramming : Command
  {
    public MovieId MovieId { get; }
    public ScreenId ScreenId { get; }
    public DateTime Date { get; }
    public IEnumerable<Dtos.Seat> Seats { get; }
    public string MovieTitle { get; }
    public string ScreenName { get; }

    public CreateDailyProgramming(DailyProgrammingId aggregateId, MovieId movieId, ScreenId screenId, DateTime date, IEnumerable<Dtos.Seat> seats, string movieTitle, string screenName) : base(aggregateId)
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
