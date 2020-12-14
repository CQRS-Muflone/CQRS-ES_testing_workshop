using System;
using System.Collections.Generic;
using CqrsMovie.Messages.Commands.Seat;
using CqrsMovie.Messages.Events.Seat;
using CqrsMovie.Seats.Domain.CommandHandlers;
using CqrsMovie.SharedKernel.Domain.Ids;
using Microsoft.Extensions.Logging.Abstractions;
using Muflone.Messages.Commands;
using Muflone.Messages.Events;

namespace CqrsMovie.Seats.Domain.Tests.Entities
{
    public class Payment_RequestPayment : CommandSpecification<RequestPayment>
    {
        private readonly PaymentId paymentId = new PaymentId(Guid.NewGuid());
        private readonly Guid correlationId = Guid.NewGuid();

        protected override IEnumerable<DomainEvent> Given()
        {
            yield break;
        }

        protected override RequestPayment When()
        {
            return new RequestPayment(paymentId, correlationId);
        }

        protected override ICommandHandler<RequestPayment> OnHandler()
        {
            return new RequestPaymentCommandHandler(Repository, new NullLoggerFactory());
        }

        protected override IEnumerable<DomainEvent> Expect()
        {
            yield return new PaymentAccepted(paymentId, correlationId);
        }
    }
}