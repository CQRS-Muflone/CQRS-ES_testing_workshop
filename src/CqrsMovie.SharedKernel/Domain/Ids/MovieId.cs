using System;
using Muflone.Core;

namespace CqrsMovie.SharedKernel.Domain.Ids
{
  public class MovieId : DomainId
  {
    public MovieId(Guid value) : base(value)
    {
    }
  }
}
