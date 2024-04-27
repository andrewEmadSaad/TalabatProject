using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class ResponseCacheService : IResponseCacheService
    {
      private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer redis) 
        {
            _database=redis.GetDatabase();
        }
        
        public async  Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null) return;

            //options to Rename Json from Pascal Case To Camel Case
            var options =new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
           
            // Serialize Response To Json
            var serializedResponse = JsonSerializer.Serialize(response,options);
            
            // Caching Response In Redis Database
            await _database.StringSetAsync(cacheKey, serializedResponse, timeToLive);

        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
           var cashedResponse=await _database.StringGetAsync(cacheKey);
           
            // We Checed Because if cached response Back Null it will be return empty redisValue
            if(cashedResponse.IsNullOrEmpty ) return null;

           return cashedResponse;
        }
    }
}
