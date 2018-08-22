using System;
using System.Linq;
using ChickenAPI.Enums;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Game.Packets
{
    /// <summary>
    ///     Game Packets only
    /// </summary>
    public class GamePacketHandler
    {
        public GamePacketHandler(Action<IPacket, IPlayerEntity> handler, Type packetType)
        {
            HandlerMethod = handler;

            PacketType = packetType;
            PacketHeader = PacketType.GetCustomAttributes(typeof(PacketHeaderAttribute), true).FirstOrDefault() as PacketHeaderAttribute;
            Identification = PacketHeader?.Identification;
            Authority = PacketHeader?.Authority ?? AuthorityType.User;
        }


        public Action<IPacket, IPlayerEntity> HandlerMethod { get; }
        public PacketHeaderAttribute PacketHeader { get; set; }
        public AuthorityType Authority { get; }
        public string Identification { get; }
        public Type PacketType { get; }
    }
}