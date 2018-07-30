namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("in_item_subpacket")]
    public class InItemSubPacketBase : PacketBase
    {
        #region Properties
        [PacketIndex(0)]
        public int Amount { get; set; }

        [PacketIndex(1)]
        public bool IsQuestRelative { get; set; }

        #endregion
    }
}
