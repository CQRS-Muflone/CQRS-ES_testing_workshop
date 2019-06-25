using System;
using System.Collections.Generic;
using CqrsMovie.SharedKernel.ReadModel;

namespace CqrsMovie.Seats.ReadModel.Dtos
{
  public class DailyProgramming : Dto
  {
    public string MovieId { get; set; }
    public string ScreenId { get; set; }
    public DateTime Date { get; set; }
    public string MovieTitle { get; set; }
    public string ScreenName { get; set; }
    public IEnumerable<Seat> Seats { get; set; }
  }
}