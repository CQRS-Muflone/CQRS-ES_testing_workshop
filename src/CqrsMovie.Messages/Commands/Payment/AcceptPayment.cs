using System;
using Muflone.Core;
using Muflone.Messages.Commands;

namespace CqrsMovie.Messages.Commands.Payment
{
    public class AcceptPayment : Command
    {
        public AcceptPayment(IDomainId aggregateId, Guid correlationId, string who = "anonymous") : base(aggregateId,
            correlationId, who)
        {
        }
    }
}