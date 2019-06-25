using System;

namespace CqrsMovie.SharedKernel.ReadModel
{
  public abstract class Dto : IEquatable<Dto>
  {
    public string Id { get; set; }

    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return Equals(obj as Dto);
    }

    public bool Equals(Dto other)
    {
      return (null != other) && (GetType() == other.GetType()) && (other.Id == Id);
    }

    public static bool operator ==(Dto Dto1, Dto Dto2)
    {
      if ((object)Dto1 == null && (object)Dto2 == null)
        return true;
      if ((object)Dto1 == null || (object)Dto2 == null)
        return false;
      return ((Dto1.GetType() == Dto2.GetType()) && (Dto1.Id == Dto2.Id));
    }

    public static bool operator !=(Dto Dto1, Dto Dto2)
    {
      return (!(Dto1 == Dto2));
    }
  }
}

