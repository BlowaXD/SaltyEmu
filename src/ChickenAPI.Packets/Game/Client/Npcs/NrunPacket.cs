﻿using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Npcs
{
    [PacketHeader("n_run")]
    public class NrunPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public short Runner { get; set; }

        [PacketIndex(1)]
        public short Type { get; set; }

        [PacketIndex(2)]
        public short Value { get; set; }

        [PacketIndex(3)]
        public int NpcId { get; set; }

        #endregion
    }
}