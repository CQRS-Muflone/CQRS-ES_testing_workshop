using System;

namespace CqrsMovie.SharedKernel.Domain.Ids
{
  public class MovieId : DomainId
  {
    public MovieId(Guid value) : base(value)
    {
    }
  }
}
