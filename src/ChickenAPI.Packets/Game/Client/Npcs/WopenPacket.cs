using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Npcs
{
    [PacketHeader("wopen")]
    public class WopenPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public long Type { get; set; }

        [PacketIndex(1)]
        public long Unknow { get; set; }

        #endregion Properties
    }
}