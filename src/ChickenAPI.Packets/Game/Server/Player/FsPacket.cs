using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Player
{
    [PacketHeader("fs")]
    public class FsPacket : PacketBase
    {
        [PacketIndex(0)]
        public FactionType Faction { get; set; }
    }
}