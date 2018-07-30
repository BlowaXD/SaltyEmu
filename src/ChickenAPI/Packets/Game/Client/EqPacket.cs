using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("eq")]
    public class EqPacket : PacketBase
    {
        public EqPacket(IPlayerEntity entity)
        {
            CharacterId = entity.Character.Id;
            VisualType = 0;
            GenderType = entity.Character.Gender;
            HairStyleType = entity.Character.HairStyle;
            HairColorType = entity.Character.HairColor;
            CharacterClassType = entity.Character.Class;
            EqList = new EqListInfo(entity.Inventory);
            EqInfo = new EqRareInfo(entity.Inventory);
        }

        [PacketIndex(0)]
        public long CharacterId { get; set; }

        [PacketIndex(1)]
        public byte VisualType { get; set; }

        [PacketIndex(2)]
        public GenderType GenderType { get; set; }

        [PacketIndex(3)]
        public HairStyleType HairStyleType { get; set; }

        [PacketIndex(4)]
        public HairColorType HairColorType { get; set; }

        [PacketIndex(5)]
        public CharacterClassType CharacterClassType { get; set; }

        [PacketIndex(6)]
        public EqListInfo EqList { get; set; }

        [PacketIndex(7, RemoveSeparator = true)]
        public EqRareInfo EqInfo { get; set; }
    }
}