using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Families
{
    [PacketHeader("glmk")]
    public class CreateFamilyPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public string FamilyName { get; set; }

        #endregion
    }
}