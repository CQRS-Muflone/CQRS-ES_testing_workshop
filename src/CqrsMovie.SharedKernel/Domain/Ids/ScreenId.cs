using System;

namespace CqrsMovie.SharedKernel.Domain.Ids
{
  public class ScreenId : DomainId
  {
    public ScreenId(Guid value) : base(value)
    {
    }
  }
}
