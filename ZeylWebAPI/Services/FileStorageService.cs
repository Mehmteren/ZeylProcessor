using System.Collections.Concurrent;
using ZeylAPI.Services.Interfaces;

namespace ZeylAPI.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly ConcurrentDictionary<string, (byte[] data, string fileName)> _storage = new();

        public Task<string> StoreAsync(byte[] data, string fileName)
        {
            var id = Guid.NewGuid().ToString();

            _storage.TryAdd(id, (data, fileName));

            return Task.FromResult(id);
        }

        public Task<(byte[] data, string fileName)?> GetAsync(string id)
        {
            if (_storage.TryGetValue(id, out var file))
            {
                return Task.FromResult<(byte[] data, string fileName)?>(file);
            }

            return Task.FromResult<(byte[] data, string fileName)?>(null);
        }
    }
}