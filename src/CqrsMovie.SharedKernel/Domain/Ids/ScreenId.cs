using System;
using Muflone.Core;

namespace CqrsMovie.SharedKernel.Domain.Ids
{
  public class ScreenId : DomainId
  {
    public ScreenId(Guid value) : base(value)
    {
    }
  }
}
