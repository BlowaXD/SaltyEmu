namespace ChickenAPI.Packets.CharacterScreen.Server
{
    [PacketHeader("clist_start")]
    public class ClistStartPacketBase : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public byte Type { get; set; }

        #endregion
    }
}