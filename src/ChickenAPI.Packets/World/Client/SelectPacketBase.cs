using System;

namespace ChickenAPI.Packets.World.Client
{
    public class SelectPacketBase : IPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public byte Slot { get; set; }

        public string Header => "select";
        public Type Type => typeof(SelectPacketBase);
        public string Serialize() => throw new NotImplementedException();

        public void Deserialize(string[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}