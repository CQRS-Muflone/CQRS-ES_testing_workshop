using System;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Messages.Events;

namespace CqrsMovie.Messages.Events.Payment
{
    public class PaymentAccepted : DomainEvent
    {
        public PaymentAccepted(DailyProgrammingId aggregateId, Guid correlationId, string who = "anonymous") : base(aggregateId,
            correlationId, who)
        {
        }
    }
}