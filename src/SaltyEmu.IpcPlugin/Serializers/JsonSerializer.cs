using System.Text;
using ChickenAPI.Core.IPC.Protocol;
using Newtonsoft.Json;

namespace SaltyEmu.Communication.Serializers
{
    public class JsonSerializer : IIpcSerializer
    {
        public byte[] Serialize(IIpcPacket packet)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(packet));
        }

        public IIpcPacket Deserialize(byte[] buffer)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(buffer)) as IIpcPacket;
        }
    }
}