using System;
using Muflone.Core;

namespace CqrsMovie.SharedKernel.Domain.Ids
{
  public class DailyProgrammingId : DomainId
  {
    public DailyProgrammingId(Guid value) : base(value)
    {
    }
  }
}
