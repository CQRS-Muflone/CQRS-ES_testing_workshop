using System;
using System.Collections.Generic;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Messages.Events;

namespace CqrsMovie.Messages.Events.Seat
{
    public class SeatsAlreadyFreed : DomainEvent
    {
        public IEnumerable<Dtos.Seat> Seats { get; }

        public SeatsAlreadyFreed(DailyProgrammingId aggregateId, Guid correlationId, IEnumerable<Dtos.Seat> seats,
            string who = "anonymous") : base(aggregateId, correlationId, who)
        {
            Seats = seats;
        }
    }
}