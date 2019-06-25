using System.Collections.Generic;
using System.Linq;
using CqrsMovie.Core.Enums;
using CqrsMovie.Messages.Dtos;

namespace CqrsMovie.Website.ReadModel
{
  public static class Helpers
  {
    public static IEnumerable<Dtos.Seat> ToReadModel(this IEnumerable<Seat> dtos, SeatState seatState)
    {
      return dtos.Select(x => new Dtos.Seat() { Number = x.Number, Row = x.Row, State = seatState });
    }
  }
}
