using System.Collections.Generic;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Messages.Events;

namespace CqrsMovie.Messages.Events.Seat
{
  public class SeatsReserved : DomainEvent
  {
    public IEnumerable<Dtos.Seat> Seats { get; }

    public SeatsReserved(DailyProgrammingId aggregateId, IEnumerable<Dtos.Seat> seats, string who = "anonymous")
        : base(aggregateId, who)
    {
      Seats = seats;
    }
  }
}