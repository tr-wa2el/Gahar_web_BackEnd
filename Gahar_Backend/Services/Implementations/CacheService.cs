using StackExchange.Redis;
using System.Text.Json;
using Gahar_Backend.Services.Interfaces;

namespace Gahar_Backend.Services.Implementations
{
  public class CacheService : ICacheService
    {
private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

  public CacheService(IConnectionMultiplexer redis)
     {
    _redis = redis;
            _db = redis.GetDatabase();
     }

      public async Task<T?> GetAsync<T>(string key)
        {
   var value = await _db.StringGetAsync(key);
       if (value.IsNullOrEmpty)
    return default;

    return JsonSerializer.Deserialize<T>(value!);
     }

   public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
       var serialized = JsonSerializer.Serialize(value);
  await _db.StringSetAsync(key, serialized, expiration);
        }

 public async Task RemoveAsync(string key)
   {
     await _db.KeyDeleteAsync(key);
 }

      public async Task RemoveByPatternAsync(string pattern)
  {
    var endpoints = _redis.GetEndPoints();
   var server = _redis.GetServer(endpoints.First());
       var keys = server.Keys(pattern: pattern);

       foreach (var key in keys)
    {
await _db.KeyDeleteAsync(key);
       }
 }
    }
}
