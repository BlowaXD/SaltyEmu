using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;
using System.Collections.Generic;

namespace ChickenAPI.Packets.Game.Client.Battle
{
    [PacketHeader("mtlist")]
    public class MultiTargetListPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public byte TargetsAmount { get; set; }

        [PacketIndex(1, RemoveSeparator = true)]
        public List<MultiTargetListSubPacket> Targets { get; set; }

        #endregion
    }
}
