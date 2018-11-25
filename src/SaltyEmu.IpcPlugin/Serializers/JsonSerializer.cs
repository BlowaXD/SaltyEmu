using System.Text;
using ChickenAPI.Core.IPC.Protocol;
using Newtonsoft.Json;

namespace SaltyEmu.Communication.Serializers
{
    public class JsonSerializer<T> : IIpcSerializer<T> where T : class
    {
        public byte[] Serialize(T packet)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(packet));
        }

        public T Deserialize(byte[] buffer)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(buffer)) as T;
        }
    }
}