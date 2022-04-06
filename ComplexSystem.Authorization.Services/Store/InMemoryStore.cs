using System.Collections.Concurrent;
using ComplexSystem.Authorization.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ComplexSystem.Authorization.Services.Store
{
    public class InMemoryStore : IStore
    {
        private readonly ConcurrentDictionary<string, string> _dictionary = new();

        public T? GetItem<T>(string key)
        {
            if (!_dictionary.ContainsKey(key))
            {
                return default;
            }

            _dictionary.TryGetValue(key, out var json);

            var value = JsonConvert.DeserializeObject<T>(json, GetSerializerSettings());

            return value;
        }

        public void SetItem<T>(string key, T value)
        {
            var json = JsonConvert.SerializeObject(value, GetSerializerSettings());

            _dictionary.AddOrUpdate(key, json, (k, v) => json);
        }

        private JsonSerializerSettings GetSerializerSettings() => new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
