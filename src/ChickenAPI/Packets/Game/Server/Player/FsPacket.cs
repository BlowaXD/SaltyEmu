using ChickenAPI.Enums.Game.Character;

namespace ChickenAPI.Game.Packets.Game.Server
{
    [PacketHeader("fs")]
    public class FsPacket : PacketBase
    {
        [PacketIndex(0)]
        public FactionType Faction { get; set; }

    }
}