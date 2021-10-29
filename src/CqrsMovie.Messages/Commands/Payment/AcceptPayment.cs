using System;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Messages.Commands;

namespace CqrsMovie.Messages.Commands.Payment
{
    public class AcceptPayment : Command
    {
        public AcceptPayment(DailyProgrammingId aggregateId, Guid correlationId, string who = "anonymous") : base(aggregateId,
            correlationId, who)
        {
        }
    }
}