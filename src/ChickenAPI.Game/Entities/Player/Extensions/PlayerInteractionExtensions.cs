using System;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Player;
using ChickenAPI.Packets.Game.Server.UserInterface;

namespace ChickenAPI.Game.Player.Extension
{
    public static class PlayerInteractionExtensions
    {
        public static MsgPacket GenerateMsgPacket(this IPlayerEntity player, string msg, MsgPacketType type)
        {
            return new MsgPacket
            {
                Message = msg,
                Type = type,
            };
        }

        public static SayPacket GenerateSayPacket(this IEntity entity, string msg, SayColorType color)
        {
            return new SayPacket
            {
                VisualId = entity.Id,
                VisualType = entity.Type,
                Message = msg,
                Type = color
            };
        }

        public static InfoPacket GenerateInfoBubble(this IPlayerEntity player, string text)
        {
            return new InfoPacket
            {
                Message = text
            };
        }
    }
}