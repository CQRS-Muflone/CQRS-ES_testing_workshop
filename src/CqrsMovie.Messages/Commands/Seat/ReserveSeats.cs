using System;
using System.Collections.Generic;
using Muflone.Core;
using Muflone.Messages.Commands;

namespace CqrsMovie.Messages.Commands.Seat
{
    public class ReserveSeats : Command
    {
        public IEnumerable<Dtos.Seat> Seats { get; }

        public ReserveSeats(IDomainId aggregateId, Guid correlationId, IEnumerable<Dtos.Seat> seats,
            string who = "anonymous") : base(aggregateId, correlationId, who)
        {
            Seats = seats;
        }
    }
}