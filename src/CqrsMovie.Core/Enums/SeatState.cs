using System;
using System.Collections.Generic;
using System.Linq;

namespace CqrsMovie.Core.Enums
{
  public class SeatState : Enumeration
  {
    public static SeatState Free = new SeatState(1, nameof(Free).ToLowerInvariant());
    public static SeatState Booked = new SeatState(2, nameof(Booked).ToLowerInvariant());
    public static SeatState Reserved = new SeatState(3, nameof(Reserved).ToLowerInvariant());

    public static IEnumerable<SeatState> List() => new[] { Free, Booked, Reserved };

    public SeatState(int id, string name)
      : base(id, name)
    {
    }

    public static SeatState FromName(string name)
    {
      var state = List().SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

      if (state == null)
        throw new Exception($"Possible values for SeatState: {string.Join(",", List().Select(s => s.Name))}");

      return state;
    }

    public static SeatState From(int id)
    {
      var state = List().SingleOrDefault(s => s.Id == id);

      if (state == null)
        throw new Exception($"Possible values for SeatState: {string.Join(",", List().Select(s => s.Name))}");

      return state;
    }
  }
}
