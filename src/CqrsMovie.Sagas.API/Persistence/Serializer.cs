using System.Threading.Tasks;
using Muflone.Saga.Persistence;
using Newtonsoft.Json;

namespace CqrsMovie.Sagas.API.Persistence
{
	public class Serializer: ISerializer
	{
		public Task<T> Deserialize<T>(string serializedData) where T : class, new()
		{
			var result = JsonConvert.DeserializeObject<T>(serializedData);
			return Task.FromResult(result);
		}

		public Task<string> Serialize<T>(T data)
		{
			var result = JsonConvert.SerializeObject(data);
			return Task.FromResult(result);
		}
	}
}
