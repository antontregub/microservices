using Facade;
using Hazelcast;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shed.CoreKit.WebApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logging
{
    public class LoggingImpl
    {
        CacheServise cacheService;
        private readonly ILogger _logger;
        public LoggingImpl(ILoggerFactory logger, CacheServise memoryCache)
        {
            _logger = logger.CreateLogger("");
            cacheService = memoryCache;

        }

        [Route("logging/post")]
        [HttpPost]
        public async Task<string> Post([FromBody] Model msg)
        {
            _logger.LogInformation($"Id: { msg.id}]");
            _logger.LogInformation($"Id: { msg.msg}]");

            await cacheService.Add(msg);
            return "";
        }

        [Route("logging/get")]
        [HttpGet]
        public async Task<string> Get()
        {
            var res =  cacheService.Get().Result;
            return res.ToString();
        }
    }

    public class CacheServise 
    {
        public CacheServise(IMemoryCache memoryCache)
        {
          
        }

        static CacheServise()
        {
        }

        public async Task Add(Model user)
        {
            await using var client = await HazelcastClientFactory.StartNewClientAsync(new HazelcastOptions
            {
                Networking =
                {
                    Addresses =
                    {
                       
                    }
                }
            });
            var map = await client.GetMapAsync<Guid, string>("lab4");

            await map.PutAsync(user.id, user.msg);
        }

        public async Task<string> Get()
        {
            await using var client = await HazelcastClientFactory.StartNewClientAsync(new HazelcastOptions
            {
                Networking =
                {
                    Addresses =
                    {

                    }
                }
            });
            var map = await client.GetMapAsync<Guid, string>("lab4");

            var mess = await map.GetEntriesAsync();

            string result = "";
            foreach (var a in mess)
            {
                result = String.Concat(result, a.Value);
            }

            return result;
        }
    }
}

