using ChickenAPI.Core.Configurations;

namespace ChickenAPI.Game.Impl
{
    public class JsonConfigurationSerializer : IConfigurationSerializer
    {
        public string Serialize<T>(T conf) where T : IConfiguration => throw new System.NotImplementedException();

        public T Deserialize<T>(string buffer) where T : IConfiguration => throw new System.NotImplementedException();
    }
}