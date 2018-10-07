using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Families
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