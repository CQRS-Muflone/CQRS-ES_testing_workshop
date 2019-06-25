using CqrsMovie.Core.Enums;

namespace CqrsMovie.Seats.ReadModel.Dtos
{
  public class Seat
  {
    public string Row { get; set; }
    public int Number { get; set; }
    public SeatState State { get; set; }
  }
}
