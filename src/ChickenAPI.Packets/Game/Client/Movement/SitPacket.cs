using System.Collections.Generic;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Movement
{
    [PacketHeader("rest")]
    public class SitPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public byte Ammout { get; set; }

        [PacketIndex(1, RemoveSeparator = true)]
        public List<SitSubPacket> Users { get; set; }

        #endregion
    }
}