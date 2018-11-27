using System;
using Autofac;
using ChickenAPI.Core.i18n;
using ChickenAPI.Core.IoC;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Player.Extension;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Game.Server.UserInterface;

namespace ChickenAPI.Game.Helpers
{
    public static class UIHelper
    {
        public static void SendChatMessageFormat(this IPlayerEntity player, ChickenI18NKey key, SayColorType color, params object[] objs)
        {
            string msg = player.GetLanguageFormat(key, objs);
            player.SendPacket(player.GenerateSayPacket(msg, color));
        }

        public static void SendChatMessage(this IPlayerEntity player, ChickenI18NKey key, SayColorType color)
        {
            string msg = player.GetLanguage(key);
            player.SendPacket(player.GenerateSayPacket(msg, color));
        }

        public static void SendChatMessage(this IPlayerEntity player, string msg, SayColorType color)
        {
            player.SendPacket(player.GenerateSayPacket(msg, color));
        }

        public static void SendTopscreenMessage(this IPlayerEntity player, string msg, MsgPacketType type)
        {
            player.SendPacket(player.GenerateMsgPacket(msg, type));
        }

        public static BSInfoPacket GenerateBSInfo(this IPlayerEntity player, byte mode, short title, short time, short text)
        {
            return new BSInfoPacket
            {
                Mode = mode,
                Title = title,
                Time = time,
                Text = text
            };
        }

        public static ChDMPacket GenerateCHDMPacket(this IPlayerEntity player, int maxhp, int angeldmg, int demondmg, int time)
        {
            return new ChDMPacket
            {
                Maxhp = maxhp,
                AngelDMG = angeldmg,
                DemonDMG = demondmg,
                Time = time
            };
        }

        public static DialogPacket GenerateDialog(this IPlayerEntity player, PacketBase acceptpacket, PacketBase refusepacket, string question)
        {
            return new DialogPacket
            {
                AcceptPacket = acceptpacket,
                RefusePacket = refusepacket,
                Question = question
            };
        }

        public static DelayPacket GenerateDelay(this IPlayerEntity player, int delay, DelayPacketType type, string argument)
        {
            return new DelayPacket
            {
                Delay = delay,
                Type = type,
                Argument = argument
            };
        }
    }
}