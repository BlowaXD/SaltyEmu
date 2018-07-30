using ChickenAPI.Enums.Game.Character;

namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("gop")]
    public class CharacterOptionPacket : PacketBase
    {
        [PacketIndex(0)]
        public CharacterOption Option { get; set; }

        [PacketIndex(1)]
        public bool IsActive { get; set; }
    }
}