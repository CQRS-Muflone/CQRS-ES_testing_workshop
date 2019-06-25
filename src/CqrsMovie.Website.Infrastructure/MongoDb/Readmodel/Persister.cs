using System;
using System.Threading.Tasks;
using CqrsMovie.SharedKernel.ReadModel;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CqrsMovie.Website.Infrastructure.MongoDb.Readmodel
{
  public class Persister : IPersister
  {
    private readonly IMongoDatabase database;
    private readonly ILogger logger;

    public Persister(IMongoDatabase database, ILoggerFactory loggerFactory)
    {
      this.database = database;
      logger = loggerFactory.CreateLogger(GetType());
    }

    public async Task<T> GetBy<T>(string id) where T : Dto
    {
      var type = typeof(T).Name;
      try
      {
        var collection = database.GetCollection<T>(type);
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await collection.CountDocumentsAsync(filter) > 0 ? (await collection.FindAsync(filter)).First() : null;
      }
      catch (Exception e)
      {
        logger.LogError($"Insert: Error saving DTO: {type}, Message: {e.Message}, StackTrace: {e.StackTrace}");
        throw;
      }
    }

    public async Task Insert<T>(T entity) where T : Dto
    {
      var type = typeof(T).Name;
      try
      {
        var collection = database.GetCollection<T>(type);
        await collection.InsertOneAsync(entity);
      }
      catch (Exception e)
      {
        logger.LogError($"Insert: Error saving DTO: {type}, Message: {e.Message}, StackTrace: {e.StackTrace}");
        throw;
      }
    }

    public async Task Update<T>(T entity) where T : Dto
    {
      var type = typeof(T).Name;
      try
      {
        var collection = database.GetCollection<T>(type);
        await collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
      }
      catch (Exception e)
      {
        logger.LogError($"Update: Error saving DTO: {type}, Message: {e.Message}, StackTrace: {e.StackTrace}");
        throw;
      }
    }

    public async Task Delete<T>(T entity) where T : Dto
    {
      var type = typeof(T).Name;
      try
      {
        var collection = database.GetCollection<T>(typeof(T).Name);
        var filter = Builders<T>.Filter.Eq("_id", entity.Id);
        await collection.FindOneAndDeleteAsync(filter);
      }
      catch (Exception e)
      {
        logger.LogError($"Delete: Error saving DTO: {type}, Message: {e.Message}, StackTrace: {e.StackTrace}");
        throw;
      }
    }
  }
}
