namespace ChickenAPI.Packets.Game.Client
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