using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.CharacterSelectionScreen.Server
{
    [PacketHeader("clist_start")]
    public class CListStartPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public byte Type { get; set; }

        #endregion
    }
}