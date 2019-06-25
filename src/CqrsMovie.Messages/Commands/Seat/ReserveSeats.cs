using System.Collections.Generic;
using Muflone.Core;
using Muflone.Messages.Commands;

namespace CqrsMovie.Messages.Commands.Seat
{
    public class ReserveSeats : Command
    {
        public IEnumerable<Dtos.Seat> Seats { get; }

        public ReserveSeats(IDomainId aggregateId, IEnumerable<Dtos.Seat> seats) 
            : base(aggregateId)
        {
            Seats = seats;
        }
    }
}