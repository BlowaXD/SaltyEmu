using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Player
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