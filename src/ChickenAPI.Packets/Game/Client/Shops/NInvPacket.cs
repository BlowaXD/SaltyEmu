using System.Collections.Generic;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Shops
{
    [PacketHeader("n_inv")]
    public class NInvPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public byte Unknown { get; set; } = 0;

        [PacketIndex(3)]
        public int ShopType { get; set; }

        [PacketIndex(4, IsOptional = true)]
        public string ShopList { get; set; }

        [PacketIndex(5, IsOptional = true, SeparatorNestedElements = " ")]
        public List<long> ShopSkills { get; set; }
    }
}