using System;
using System.Collections.Generic;
using CqrsMovie.SharedKernel.Domain.Ids;
using Muflone.Messages.Commands;

namespace CqrsMovie.Messages.Commands.Seat
{
    public class RequestPayment : Command
    {
        public readonly PaymentId PaymentId;
        public IEnumerable<Dtos.Seat> Seats { get; }

        public RequestPayment(PaymentId aggregateId, Guid correlationId, IEnumerable<Dtos.Seat> seats, string who = "anonymous")
            : base(aggregateId, correlationId, who)
        {
            PaymentId = aggregateId;
            Seats = seats;
        }
    }
}