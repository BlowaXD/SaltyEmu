namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("glmk")]
    public class CreateFamilyPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public string Name { get; set; }

        #endregion
    }
}