namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("eqinfo")]
    public class EquipmentInfoPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public byte Type { get; set; }

        [PacketIndex(1)]
        public short Slot { get; set; }

        [PacketIndex(3)]
        public long? ShopOwnerId { get; set; }

        #endregion
    }
}