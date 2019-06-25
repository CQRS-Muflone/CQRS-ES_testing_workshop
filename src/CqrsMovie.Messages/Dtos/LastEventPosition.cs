using CqrsMovie.SharedKernel.ReadModel;

namespace CqrsMovie.Messages.Dtos
{
  public class LastEventPosition : Dto
  {
    public long CommitPosition { get; set; }
    public long PreparePosition { get; set; }
  }
}
