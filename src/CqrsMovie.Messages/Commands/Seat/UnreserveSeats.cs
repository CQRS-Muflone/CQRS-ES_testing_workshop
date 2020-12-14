using System.Collections.Generic;
using Muflone.Core;
using Muflone.Messages.Commands;

namespace CqrsMovie.Messages.Commands.Seat
{
    public class UnreserveSeats : Command
    {
        public IEnumerable<Dtos.Seat> Seats { get; }

        public UnreserveSeats(IDomainId aggregateId, IEnumerable<Dtos.Seat> seats)
            : base(aggregateId)
        {
            Seats = seats;
        }
    }
}