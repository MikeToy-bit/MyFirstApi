using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MyFirstApi.Services
{
    public interface IT_SYS_TokenBlacklistService
    {
        Task AddToBlacklist(string token, DateTime expiration);
        Task<bool> IsBlacklisted(string token);
    }

    public class T_SYS_TokenBlacklistService : IT_SYS_TokenBlacklistService
    {
        private readonly ConcurrentDictionary<string, DateTime> _blacklist = new();

        public Task AddToBlacklist(string token, DateTime expiration)
        {
            _blacklist.TryAdd(token, expiration);
            return Task.CompletedTask;
        }

        public Task<bool> IsBlacklisted(string token)
        {
            if (_blacklist.TryGetValue(token, out var expiration))
            {
                if (DateTime.UtcNow > expiration)
                {
                    _blacklist.TryRemove(token, out _);
                    return Task.FromResult(false);
                }
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
} 