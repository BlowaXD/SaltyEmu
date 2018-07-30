﻿namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("walk")]
    public class WalkPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public short XCoordinate { get; set; }

        [PacketIndex(1)]
        public short YCoordinate { get; set; }

        [PacketIndex(2)]
        public short Unknown { get; set; }

        [PacketIndex(3)]
        public byte Speed { get; set; }

        #endregion
    }
}