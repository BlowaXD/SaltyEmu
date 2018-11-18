using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Player
{
    [PacketHeader("compl")]
    public class ComplimentPacket : PacketBase
    {
        #region Properties

        [PacketIndex(1)]
        public long CharacterId { get; set; }

        #endregion
    }
}