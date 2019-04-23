﻿using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Movement
{
    [PacketHeader("dir")]
    public class DirectionPacket : PacketBase
    {
        #region Properties

        [PacketIndex(1)]
        public int Option { get; set; }

        [PacketIndex(0)]
        public DirectionType DirectionType { get; set; }

        [PacketIndex(2)]
        public long CharacterId { get; set; }

        #endregion
    }
}