using System;
using System.Collections.Generic;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Messages.Events;

namespace CqrsMovie.Messages.Events.Seat
{
    public class PaymentAccepted : DomainEvent
    {
        public readonly PaymentId PaymentId;
        public readonly IEnumerable<Dtos.Seat> Seats;

        public PaymentAccepted(PaymentId aggregateId, IEnumerable<Dtos.Seat> seats, Guid correlationId, string who = "anonymous") : base(aggregateId, correlationId, who)
        {
            PaymentId = aggregateId;
            Seats = seats;
        }
    }
}