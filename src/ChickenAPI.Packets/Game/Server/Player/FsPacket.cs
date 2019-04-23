using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Player
{
    [PacketHeader("fs")]
    public class FsPacket : PacketBase
    {
        [PacketIndex(0)]
        public FactionType Faction { get; set; }
    }
}