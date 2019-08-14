using System.Collections.Generic;
using CqrsMovie.Core.Enums;
using Muflone.Core;

namespace CqrsMovie.Seats.Domain.Entities
{
  public class Seat: ValueObject
  {
    public Seat(string row, int number, SeatState state)
    {
      Row = row;
      Number = number;
      State = state;
    }

    public string Row { get;  }
    public int Number { get; }
    public SeatState State { get; }


    protected override IEnumerable<object> GetEqualityComponents()
    {
      yield return Row;
      yield return Number;
      yield return State;
    }
  }
}
