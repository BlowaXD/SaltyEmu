using ChickenAPI.Core.i18n;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Player.Extension;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Game.Client.Inventory;
using ChickenAPI.Packets.Game.Server.Inventory;
using ChickenAPI.Packets.Game.Server.UserInterface;

namespace ChickenAPI.Game.Helpers
{
    public static class UIHelper
    {

        public static void SendGuri(this IPlayerEntity player, byte type, byte argument, int value = 0)
        {
            player.SendPacket(player.GenerateGuriPacket(type, argument, value));
        }
        public static ServerGuriPacket GenerateGuriPacket(this IPlayerEntity player, byte type, byte argument, int value = 0)
        {
            switch (type)
            {
                case 2:
                    return new ServerGuriPacket
                    {
                        Type = 2, Argument = argument, VisualId = player.Id
                    };
                case 6:
                    return new ServerGuriPacket
                    {
                        Type = 6, Argument = 1, VisualId = player.Id, Value = 0, Data = 0
                    };
                case 10:
                    return new ServerGuriPacket
                    {
                        Type = 10, Argument = argument, VisualId = value, Value = player.Id
                    };
                case 15:
                    return new ServerGuriPacket
                    {
                        Type = 15, Argument = argument , VisualId = 0, Data = 0
                    };
                default:
                    return new ServerGuriPacket
                    {
                        Type = type, Argument = argument, VisualId = player.Id, Value = value
                    };
            }
        }

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

        public static void GenerateBSInfo(this IPlayerEntity player, byte mode, short title, short time, short text)
        {
            player.SendPacket(player.GenerateBSInfoPacket(mode, title, time, text));
        }

        public static void GenerateCHDM(this IPlayerEntity player, int maxhp, int angeldmg, int demondmg, int time)
        {
            player.SendPacket(player.GenerateCHDMPacket(maxhp, angeldmg, demondmg, time));
        }

        public static void GenerateDialog(this IPlayerEntity player, PacketBase acceptpacket, PacketBase refusepacket, string question)
        {
            player.SendPacket(player.GenerateDialogPacket(acceptpacket, refusepacket, question));
        }

        public static void GenerateDelay(this IPlayerEntity player, int delay, DelayPacketType type, string argument)
        {
            player.SendPacket(player.GenerateDelayPacket(delay, type, argument));
        }

        public static BSInfoPacket GenerateBSInfoPacket(this IPlayerEntity player, byte mode, short title, short time, short text)
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

        public static DialogPacket GenerateDialogPacket(this IPlayerEntity player, PacketBase acceptpacket, PacketBase refusepacket, string question)
        {
            return new DialogPacket
            {
                AcceptPacket = acceptpacket,
                RefusePacket = refusepacket,
                Question = question
            };
        }

        public static DelayPacket GenerateDelayPacket(this IPlayerEntity player, int delay, DelayPacketType type, string argument)
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