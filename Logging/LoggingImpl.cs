using Facade;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shed.CoreKit.WebApi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        Dictionary<Guid, string> temp = new Dictionary<Guid, string>();

        [Route("post")]
        [HttpPost]
        public string Post([FromBody] Model msg)
        {
            _logger.LogInformation($"Id: { msg.id}]");

            cacheService.AddUser(msg);
            return "";
        }

        [Route("get")]
        [HttpGet]
        public string Get()
        {
            var res = cacheService.GetUser().Result;
            return res;
        }
    }

    public class CacheServise
    {
        private IMemoryCache cache;
        protected static readonly ConcurrentDictionary<System.Guid, bool> _allKeys;
        public CacheServise( IMemoryCache memoryCache)
        {
            cache = memoryCache;
        }

        static CacheServise()
        {
            _allKeys = new ConcurrentDictionary<System.Guid, bool>();
        }
       
        public async Task AddUser(Model user)
        {
                cache.Set(user.id, user, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            _allKeys.TryAdd(user.id, true);
        }

        public async Task<string> GetUser()
        {
            Model user = null;
            var aa = _allKeys.Keys.ToList();
            string result = "";
            foreach(var a in aa)
            {
                cache.TryGetValue(a, out user);
                result = String.Concat(result, user.msg);
            }

            return result;
        }
    }
}

