using System.Threading.Tasks;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game._i18n;
using ChickenAPI.Packets.Game.Server.Player;
using ChickenAPI.Packets.Game.Server.UserInterface;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class PlayerInteractionExtensions
    {
        public static Task SendMessageAsync(this IPlayerEntity player, PlayerMessages message, MsgPacketType type)
        {
            return player.SendPacketAsync(player.GenerateMsgPacket(message, type));
        }

        public static Task SendModalAsync(this IPlayerEntity player, string message, ModalPacketType type)
        {
            return player.SendPacketAsync(player.GenerateModalPacket(message, type));
        }

        public static Task SendModalAsync(this IPlayerEntity player, PlayerMessages message, ModalPacketType type)
        {
            return player.SendModalAsync(player.GetLanguage(message), type);
        }

        public static ModalPacket GenerateModalPacket(this IPlayerEntity player, string message, ModalPacketType type)
        {
            return new ModalPacket
            {
                Type = type,
                Message = message,
            };
        }

        public static MsgPacket GenerateMsgPacket(this IPlayerEntity player, PlayerMessages message, MsgPacketType type)
        {
            return new MsgPacket
            {
                Type = type,
                Message = player.GetLanguage(message)
            };
        }

        public static MsgPacket GenerateMsgPacket(this IPlayerEntity player, string msg, MsgPacketType type) =>
            new MsgPacket
            {
                Message = msg,
                Type = type
            };

        public static SayPacket GenerateGlobalSayPacket(this IPlayerEntity player, string prefix, string message)
        {
            return player.GenerateSayPacket($"{prefix}:[{player.Character.Name}]: {message}", SayColorType.Purple);
        }

        public static SayPacket GenerateSayPacket(this IEntity entity, string msg, SayColorType color) =>
            new SayPacket
            {
                VisualId = entity.Id,
                VisualType = entity.Type,
                Message = msg,
                Type = color
            };

        public static InfoPacket GenerateInfoBubble(this IPlayerEntity player, string text) =>
            new InfoPacket
            {
                Message = text
            };
    }
}