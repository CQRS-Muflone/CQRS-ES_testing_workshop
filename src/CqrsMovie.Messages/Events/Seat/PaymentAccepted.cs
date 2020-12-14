using System;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Messages.Events;

namespace CqrsMovie.Messages.Events.Seat
{
    public class PaymentAccepted : DomainEvent
    {
        public readonly PaymentId PaymentId;

        public PaymentAccepted(PaymentId aggregateId, Guid correlationId, string who = "anonymous") : base(aggregateId, correlationId, who)
        {
            PaymentId = aggregateId;
        }
    }
}