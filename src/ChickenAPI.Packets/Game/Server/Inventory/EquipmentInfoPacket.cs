using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("eqinfo")]
    public class EquipmentInfoPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public byte Type { get; set; } // todo more information

        [PacketIndex(1)]
        public short Slot { get; set; }

        [PacketIndex(2)]
        public long? ShopOwnerId { get; set; }

        #endregion
    }
}