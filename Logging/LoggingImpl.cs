using Interfaces;
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
    public class LoggingImpl : ILogging
    {
        UserService userService;
        private readonly ILogger _logger;
        public LoggingImpl(ILoggerFactory logger, UserService memoryCache)
        {
            _logger = logger.CreateLogger("");
            userService = memoryCache;

        }
        Dictionary<Guid, string> temp = new Dictionary<Guid, string>();
        public string Post([FromBody] Model msg)
        {
            _logger.LogInformation($"Id: { msg.id}]");

            userService.AddUser(msg);
            //temp.Add(msg.id, msg.msg);
            return "1";
        }

        public string Get()
        {
            var res = userService.GetUser().Result;
            return res;
        }
    }

    public class UserService
    {
        private IMemoryCache cache;
        protected static readonly ConcurrentDictionary<System.Guid, bool> _allKeys;
        public UserService( IMemoryCache memoryCache)
        {
            cache = memoryCache;
            //_allKeys = new ConcurrentDictionary<System.Guid, bool>();
        }


        static UserService()
        {
            _allKeys = new ConcurrentDictionary<System.Guid, bool>();
        }
        //public async Task<IEnumerable<Model>> GetUsers()
        //{
        //    return await db.Users.ToListAsync();
        //}

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
            //cache.TryGetValue(user.id, out user);
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

