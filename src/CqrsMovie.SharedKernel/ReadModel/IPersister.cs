using System;
using System.Threading.Tasks;

namespace CqrsMovie.SharedKernel.ReadModel
{
  public interface IPersister
  {
    Task<T> GetBy<T>(string id) where T : Dto;
    Task Insert<T>(T entity) where T : Dto;
    Task Update<T>(T entity) where T : Dto;
    Task Delete<T>(T entity) where T : Dto;
  }
}
