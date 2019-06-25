using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CqrsMovie.SharedKernel.ReadModel;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CqrsMovie.Website.Infrastructure.MongoDb.Readmodel
{
  public abstract class Queries<T> : IQueries<T> where T : Dto
  {
    protected readonly IMongoDatabase Database;

    protected Queries(IMongoDatabase database)
    {
      Database = database;
    }

    public async Task<T> GetById(string id)
    {
      var collection = Database.GetCollection<T>(typeof(T).Name);
      var filter = Builders<T>.Filter.Eq("_id", id);
      return await collection.CountDocumentsAsync(filter) > 0 ? (await collection.FindAsync(filter)).First() : null;
    }

    public async Task<PagedResult<T>> GetByFilter(Expression<Func<T, bool>> query, int page, int pageSize)
    {
      //TODO a minimum errors handling would be nice in real life
      if (--page < 0)
        page = 0;
      var collection = Database.GetCollection<T>(typeof(T).Name);
      var count = await collection.AsQueryable().Where(query).CountAsync();
      var results = await collection.AsQueryable().Where(query).Skip(page * pageSize).Take(pageSize).ToListAsync();
      return new PagedResult<T>(results, page, pageSize, count);
    }
  }
}