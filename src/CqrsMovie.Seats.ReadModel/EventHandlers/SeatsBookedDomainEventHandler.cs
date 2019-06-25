using System;
using System.Linq;
using System.Threading.Tasks;
using CqrsMovie.Core.Enums;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.Seats.ReadModel.Dtos;
using CqrsMovie.SharedKernel.ReadModel;
using Microsoft.Extensions.Logging;

namespace CqrsMovie.Seats.ReadModel.EventHandlers
{
  public class SeatsBookedDomainEventHandler : DomainEventHandler<SeatsBooked>
  {
    public SeatsBookedDomainEventHandler(IPersister persister, ILoggerFactory loggerFactory)
        : base(persister, loggerFactory)
    {
    }

    public override async Task Handle(SeatsBooked command)
    {
      var bookingDailyProgramming = await Persister.GetBy<DailyProgramming>(command.AggregateId.ToString());

      if (bookingDailyProgramming == null)
        throw new Exception("Update ReadModel by rewind all events :-)");

      command.Seats.ToList().ForEach(seat =>
      {
        var seatToUpdate =
                  bookingDailyProgramming.Seats.FirstOrDefault(s =>
                      s.Row.Equals(seat.Row) && s.Number.Equals(seat.Number));

        if (seatToUpdate != null)
          seatToUpdate.State = SeatState.Booked;
      });
      await Persister.Update(bookingDailyProgramming);
    }
  }
}