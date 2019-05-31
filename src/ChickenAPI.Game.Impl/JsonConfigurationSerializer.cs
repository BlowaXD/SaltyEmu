using System;
using ChickenAPI.Core.Configurations;
using Newtonsoft.Json;

namespace ChickenAPI.Game.Impl
{
    public class JsonConfigurationSerializer : IConfigurationSerializer
    {
        public string Serialize<T>(T conf) where T : IConfiguration => JsonConvert.SerializeObject(conf);

        public T Deserialize<T>(string buffer) where T : IConfiguration => JsonConvert.DeserializeObject<T>(buffer);
    }
}