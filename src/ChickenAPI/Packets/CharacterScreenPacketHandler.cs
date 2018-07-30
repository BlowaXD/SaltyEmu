using System;
using System.Linq;
using ChickenAPI.Enums;
using ChickenAPI.Game.Network;

namespace ChickenAPI.Packets
{
    /// <summary>
    ///     Packet that does not need to be ingame to be handled
    /// </summary>
    public class CharacterScreenPacketHandler
    {
        #region Instantiation

        public CharacterScreenPacketHandler(Action<IPacket, ISession> handler, Type packetBaseParameterType)
        {
            Handler = handler;
            PacketType = packetBaseParameterType;
            PacketHeader = PacketType.GetCustomAttributes(typeof(PacketHeaderAttribute), true).FirstOrDefault() as PacketHeaderAttribute;
            Identification = PacketHeader?.Identification;
            Authority = PacketHeader?.Authority ?? AuthorityType.User;
            if (PacketHeader != null)
            {
                NeedCharacter = PacketHeader.NeedCharacter;
            }
        }

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