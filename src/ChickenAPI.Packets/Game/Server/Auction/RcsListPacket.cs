using System.Collections.Generic;
using ChickenAPI.Packets.Attributes;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace ChickenAPI.Packets.Game.Server.Auction
{
    [PacketHeader("rc_slist")]
    public class RcsListPacket : PacketBase
    {
        [PacketIndex(0)]
        public int PageNumber { get; set; }

        [PacketIndex(1, SeparatorNestedElements = " ")]
        public List<RcsListElementPacket> Items { get; set; }

        [PacketHeader("rc_slist_element")]
        public class RcsListElementPacket : PacketBase
        {
            [PacketIndex(0)]
            public long AuctionId { get; set; }

            [PacketIndex(1, SeparatorBeforeProperty = "|")]
            public long OwnerId { get; set; }

            [PacketIndex(2, SeparatorBeforeProperty = "|")]
            public long ItemId { get; set; }

            [PacketIndex(3, SeparatorBeforeProperty = "|")]
            public long SoldAmount { get; set; }

            [PacketIndex(4, SeparatorBeforeProperty = "|")]
            public long Amount { get; set; }

            [PacketIndex(5, SeparatorBeforeProperty = "|")]
            public bool IsPackage { get; set; }

            [PacketIndex(6, SeparatorBeforeProperty = "|")]
            public long Price { get; set; }

            [PacketIndex(7, SeparatorNestedElements = "|")]
            public long MinutesLeft { get; set; }

            [PacketIndex(8, SeparatorBeforeProperty = "|")]
            public bool IsSellerUsingMedal { get; set; }

            [PacketIndex(9, SeparatorBeforeProperty = "|")]
            public long Unknown { get; set; }

            [PacketIndex(10, SeparatorBeforeProperty = "|")]
            public long Rarity { get; set; }

            [PacketIndex(11, SeparatorBeforeProperty = "|")]
            public long Upgrade { get; set; }

            [PacketIndex(12, SeparatorBeforeProperty = "|")]
            public EInfoPacket EInfo { get; set; }
        }
    }
}