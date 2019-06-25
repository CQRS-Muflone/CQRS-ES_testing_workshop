using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CqrsMovie.Website.ReadModel.Abstracts;
using CqrsMovie.Website.ReadModel.Dtos;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CqrsMovie.Website.Infrastructure.MongoDb.Readmodel
{
  public class DailyProgrammingQueries : Queries<DailyProgramming>, IDailyProgrammingQueries
  {
    public DailyProgrammingQueries(IMongoDatabase database)
      : base(database)
    {
    }

    //TODO magari un DTO con già i dati aggregati
    public async Task<IEnumerable<DailyProgramming>> SearchByDate(DateTime date)
    {
      var collection = Database.GetCollection<DailyProgramming>(typeof(DailyProgramming).Name);
      return await collection.AsQueryable().Where(x=>x.Date==date).ToListAsync();
    }
  }
}