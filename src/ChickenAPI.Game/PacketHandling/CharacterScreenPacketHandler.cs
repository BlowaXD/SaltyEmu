using System;
using ChickenAPI.Enums;
using ChickenAPI.Game.Network;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Game.PacketHandling
{
    /// <summary>
    ///     Packet that does not need to be ingame to be handled
    /// </summary>
    public class CharacterScreenPacketHandler
    {
        #region Instantiation

        #endregion

        #region Properties

        public Action<IPacket, ISession> Handler { get; }
        public PacketHeaderAttribute PacketHeader { get; }
        public AuthorityType Authority { get; }
        public string Identification { get; }
        public Type PacketType { get; }
        public bool NeedCharacter { get; }

        #endregion
    }
}