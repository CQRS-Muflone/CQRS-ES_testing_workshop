using System.Collections.Generic;
using System.Linq;
using CqrsMovie.Core.Enums;
using CqrsMovie.Seats.Domain.Entities;

namespace CqrsMovie.Seats.Domain
{
  public static class Helpers
  {
    public static IList<Seat> ToEntity(this IEnumerable<Messages.Dtos.Seat> dtos, SeatState seatState)
    {
      return dtos.Select(x => new Seat(x.Row, x.Number, seatState)).ToList();
    }

    public static IList<Messages.Dtos.Seat> ToDto(this IEnumerable<Seat> entities)
    {
      return entities.Select(x => new Messages.Dtos.Seat() { Row = x.Row, Number = x.Number }).ToList();
    }

    public static Seat ToEntity(this Messages.Dtos.Seat seat, SeatState seatState)
    {
      return new Seat(seat.Row, seat.Number, seatState);
    }
  }
}
