using System;
using System.Collections.Generic;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Core;

namespace CqrsMovie.Seats.Domain.Entities
{
    public class Payment : AggregateRoot
    {
        protected Payment()
        {}

        internal static Payment CreatePayment(PaymentId paymentId, IEnumerable<Messages.Dtos.Seat> seatsToBook, Guid correlationId)
        {
            return new Payment(paymentId, seatsToBook, correlationId);
        }

        protected Payment(PaymentId paymentId, IEnumerable<Messages.Dtos.Seat> seatsToBook, Guid correlationId)
        {
            RaiseEvent(new PaymentAccepted(paymentId, seatsToBook, correlationId));
        }

        private void Apply(PaymentAccepted @event)
        {
            Id = @event.AggregateId;
        }
    }
}