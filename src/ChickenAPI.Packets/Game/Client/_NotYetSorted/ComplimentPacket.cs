using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client._NotYetSorted
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