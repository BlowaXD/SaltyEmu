using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace ChickenAPI.Packets.Game.Server.Chat
{
    [PacketHeader("sayitem")]
    public class SayItemPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public byte OratorSlot { get; set; }


        [PacketIndex(3)]
        public string GlobalPrefix { get; set; }

        [PacketIndex(4, SeparatorBeforeProperty = "^")]
        public string CharacterName { get; set; }

        [PacketIndex(5)]
        public string ItemName { get; set; }

        /// <summary>
        /// Spaces should be replaced by '^'
        /// </summary>
        [PacketIndex(6, IsOptional = true, SeparatorBeforeProperty = "^")]
        public string Message { get; set; }

        [PacketIndex(6, IsOptional = true, SeparatorBeforeProperty = " ")]
        public SayItemSubPacket ItemData { get; set; }

        [PacketHeader("say_item_subpacket")]
        public class SayItemSubPacket : PacketBase
        {
            // disgusting but it's entwell
            // :hap:
            [PacketIndex(0, IsOptional = true, SeparatorBeforeProperty = "ItemInfo ")]
            public long? IconId { get; set; }

            [PacketIndex(1, IsOptional = true, SeparatorBeforeProperty = " ")]
            public EInfoPacket EquipmentInfo { get; set; }
        }
    }
}