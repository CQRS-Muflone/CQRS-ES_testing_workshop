using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CqrsMovie.SharedKernel.ReadModel;
using CqrsMovie.Website.ReadModel.Dtos;

namespace CqrsMovie.Website.ReadModel.Abstracts
{
  public interface IDailyProgrammingQueries : IQueries<DailyProgramming>
  {
    //We should have used the specification pattern, but this a demo :)
    Task<IEnumerable<DailyProgramming>> SearchByDate(DateTime date);
  }
}