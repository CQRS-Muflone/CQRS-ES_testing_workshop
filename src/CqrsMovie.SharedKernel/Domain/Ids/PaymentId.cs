using System;
using Muflone.Core;

namespace CqrsMovie.SharedKernel.Domain.Ids
{
    public sealed class PaymentId : DomainId
    {
        public PaymentId(Guid value) : base(value)
        {
        }
    }
}