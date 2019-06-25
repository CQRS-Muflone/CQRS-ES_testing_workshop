using Muflone;
using Muflone.Core;

namespace CqrsMovie.SharedKernel.Domain
{
  public abstract class AggregateRoot : AggregateBase
  {
    protected AggregateRoot()
    {
    }

    protected AggregateRoot(IRouteEvents handler)
      : base(handler)
    {
    }
  }
}
