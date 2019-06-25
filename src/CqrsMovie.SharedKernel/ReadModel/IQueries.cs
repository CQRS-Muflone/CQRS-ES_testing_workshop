using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CqrsMovie.SharedKernel.ReadModel
{
  public interface IQueries<T> where T : Dto
  {
    Task<T> GetById(string id);
    Task<PagedResult<T>> GetByFilter(Expression<Func<T, bool>> query, int page, int pageSize);
  }
}
