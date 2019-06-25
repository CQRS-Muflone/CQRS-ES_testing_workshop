using System;
using Muflone.Core;

namespace CqrsMovie.SharedKernel.Domain.Ids
{
  public abstract class DomainId : IDomainId, IEquatable<DomainId>
  {
    public Guid Value { get; }

    protected DomainId(Guid value)
    {
      Value = value;
    }

    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return Equals(obj as DomainId);
    }

    public bool Equals(DomainId other)
    {
      return (null != other) && (GetType() == other.GetType()) && (other.Value == Value);
    }

    public static bool operator ==(DomainId DomainId1, DomainId DomainId2)
    {
      if ((object)DomainId1 == null && (object)DomainId2 == null)
        return true;

      if ((object)DomainId1 == null || (object)DomainId2 == null)
        return false;

      return ((DomainId1.GetType() == DomainId2.GetType()) && (DomainId1.Value == DomainId2.Value));
    }

    public static bool operator !=(DomainId DomainId1, DomainId DomainId2)
    {
      return (!(DomainId1 == DomainId2));
    }

    public override string ToString()
    {
      return Value.ToString();
    }
  }
}
