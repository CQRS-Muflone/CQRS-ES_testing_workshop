using System;
using Muflone.Core;
using Muflone.Messages.Events;

namespace CqrsMovie.Messages.Events.Payment
{
    public class PaymentAccepted : DomainEvent
    {
        public PaymentAccepted(IDomainId aggregateId, Guid correlationId, string who = "anonymous") : base(aggregateId,
            correlationId, who)
        {
        }
    }
}