using System;

namespace CqrsMovie.SharedKernel.Domain
{
  public abstract class Entity: IEquatable<Entity>
  {
    internal readonly Guid Id;
    
    protected Entity()
    {
    }

    protected Entity(Guid id)
    {
      Id = id;
    }

    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return Equals(obj as Entity);
    }

    public bool Equals(Entity other)
    {
      return (null != other) && (GetType() == other.GetType()) && (other.Id == Id);
    }

    public static bool operator ==(Entity entity1, Entity entity2)
    {
      if ((object)entity1 == null && (object)entity2 == null)
        return true;

      if ((object)entity1 == null || (object)entity2 == null)
        return false;

      return ((entity1.GetType() == entity2.GetType()) && (entity1.Id == entity2.Id));
    }

    public static bool operator !=(Entity entity1, Entity entity2)
    {
      return (!(entity1 == entity2));
    }
  }
}
