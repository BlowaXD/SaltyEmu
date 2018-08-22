using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Player
{
    [PacketHeader("levelup")]
    public class LevelUpPacket : PacketBase
    {
        [PacketIndex(0)]
        public long CharacterId { get; set; }
    }
}