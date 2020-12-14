using System;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Core;

namespace CqrsMovie.Seats.Domain.Entities
{
    public class Payment : AggregateRoot
    {
        protected Payment()
        {}

        internal static Payment CreatePayment(PaymentId paymentId, Guid correlationId)
        {
            return new Payment(paymentId, correlationId);
        }

        protected Payment(PaymentId paymentId, Guid correlationId)
        {
            RaiseEvent(new PaymentAccepted(paymentId, correlationId));
        }

        private void Apply(PaymentAccepted @event)
        {
            Id = @event.AggregateId;
        }
    }
}