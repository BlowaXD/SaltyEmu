using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client._NotYetSorted
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