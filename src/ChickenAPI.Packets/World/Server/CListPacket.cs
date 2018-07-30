using System;
using System.Text;

namespace ChickenAPI.Packets.World.Server
{
    public class CListPacket : IPacket
    {
        public string Header => "clist";
        public Type Type => typeof(CListPacket);

        public string Serialize()
        {
            var packetBuilder = new StringBuilder();


            return packetBuilder.ToString();
        }

        public void Deserialize(string[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}