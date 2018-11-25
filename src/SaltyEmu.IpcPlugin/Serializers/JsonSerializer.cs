using System.Text;
using ChickenAPI.Core.IPC.Protocol;
using Newtonsoft.Json;

namespace SaltyEmu.Communication.Serializers
{
    public class JsonSerializer : IIpcSerializer
    {
        public byte[] Serialize<T>(T packet)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(packet));
        }

        public T Deserialize<T>(byte[] buffer)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(buffer));
        }
    }
}