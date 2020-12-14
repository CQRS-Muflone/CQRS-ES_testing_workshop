using System;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Messages.Commands;

namespace CqrsMovie.Messages.Commands.Seat
{
    public class RequestPayment : Command
    {
        public readonly PaymentId PaymentId;

        public RequestPayment(PaymentId aggregateId, Guid correlationId, string who = "anonymous") : base(aggregateId, correlationId, who)
        {
            PaymentId = aggregateId;
        }
    }
}