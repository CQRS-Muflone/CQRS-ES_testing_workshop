using System;
using System.Collections.Generic;
using Muflone.Core;
using Muflone.Messages.Events;

namespace CqrsMovie.Messages.Events.Seat
{
    public class SeatsBooked : DomainEvent
    {
        public IEnumerable<Dtos.Seat> Seats { get; }

        public SeatsBooked(IDomainId aggregateId, Guid correlationId, IEnumerable<Dtos.Seat> seats,
            string who = "anonymous") : base(aggregateId, correlationId, who)
        {
            Seats = seats;
        }
    }
}