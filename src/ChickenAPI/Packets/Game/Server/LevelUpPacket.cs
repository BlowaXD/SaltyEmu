namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("levelup")]
    public class LevelUpPacket : PacketBase
    {
        [PacketIndex(0)]
        public long CharacterId { get; set; }
    }
}