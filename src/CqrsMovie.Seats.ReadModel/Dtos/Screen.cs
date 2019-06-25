using System.Collections.Generic;
using CqrsMovie.SharedKernel.ReadModel;

namespace CqrsMovie.Seats.ReadModel.Dtos
{
  public class Screen : Dto
  {
    public string Name { get; set; }
    public IEnumerable<Seat> Seats { get; set; }
  }
}