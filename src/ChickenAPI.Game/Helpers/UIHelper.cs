using System.Threading.Tasks;
using ChickenAPI.Core.i18n;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game._i18n;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Game.Client.Player;
using ChickenAPI.Packets.Game.Server.UserInterface;

namespace ChickenAPI.Game.Helpers
{
    public static class UIHelper
    {
        public static Task SendGuri(this IPlayerEntity player, GuriPacketType type, byte argument, int value = 0) => player.SendPacketAsync(player.GenerateGuriPacket(type, argument, value));

        public static ClientGuriPacket GenerateGuriPacket(this IPlayerEntity player, GuriPacketType type, byte argument, int value = 0)
        {
            switch (type)
            {
                case GuriPacketType.Unknow:
                    return new ClientGuriPacket
                    {
                        Type = 2,
                        Argument = argument,
                        VisualId = player.Id
                    };

                case GuriPacketType.Unknow2:
                    return new ClientGuriPacket
                    {
                        Type = 6,
                        Argument = 1,
                        VisualId = player.Id,
                        Value = 0,
                        Data = 0
                    };

                case GuriPacketType.Unknow3:
                    return new ClientGuriPacket
                    {
                        Type = 10,
                        Argument = argument,
                        VisualId = value,
                        Value = player.Id
                    };

                case GuriPacketType.Unknow4:
                    return new ClientGuriPacket
                    {
                        Type = 15,
                        Argument = argument,
                        VisualId = 0,
                        Data = 0
                    };

                default:
                    return new ClientGuriPacket
                    {
                        Type = (int)type,
                        Argument = argument,
                        VisualId = player.Id,
                        Value = value
                    };
            }
        }

        public static Task SendChatMessageFormat(this IPlayerEntity player, PlayerMessages key, SayColorType color, params object[] objs)
        {
            string msg = player.GetLanguageFormat(key, objs);
            return player.SendPacketAsync(player.GenerateSayPacket(msg, color));
        }

        public static Task SendChatMessageAsync(this IPlayerEntity player, PlayerMessages key, SayColorType color)
        {
            string msg = player.GetLanguage(key);
            return player.SendPacketAsync(player.GenerateSayPacket(msg, color));
        }

        public static Task SendChatMessageAsync(this IPlayerEntity player, string msg, SayColorType color) => player.SendPacketAsync(player.GenerateSayPacket(msg, color));

        public static Task SendTopscreenMessage(this IPlayerEntity player, string msg, MsgPacketType type) => player.SendPacketAsync(player.GenerateMsgPacket(msg, type));

        public static Task GenerateBSInfo(this IPlayerEntity player, byte mode, short title, short time, short text) => player.SendPacketAsync(player.GenerateBSInfoPacket(mode, title, time, text));

        public static Task GenerateCHDM(this IPlayerEntity player, int maxhp, int angeldmg, int demondmg, int time) =>
            player.SendPacketAsync(player.GenerateCHDMPacket(maxhp, angeldmg, demondmg, time));

        public static Task SendDialog(this IPlayerEntity player, PacketBase acceptpacket, PacketBase refusepacket, string question) =>
            player.SendPacketAsync(player.GenerateDialogPacket(acceptpacket, refusepacket, question));

        public static Task SendQuestionAsync(this IPlayerEntity player, PacketBase acceptpacket, string question) => player.SendPacketAsync(player.GenerateQnaPacket(acceptpacket, question));

        public static Task GenerateDelay(this IPlayerEntity player, int delay, DelayPacketType type, string argument) => player.SendPacketAsync(player.GenerateDelayPacket(delay, type, argument));

        public static QnaPacket GenerateQnaPacket(this IPlayerEntity player, PacketBase acceptpacket, string question) =>
            new QnaPacket
            {
                AcceptPacket = acceptpacket,
                Question = question
            };

        public static BSInfoPacket GenerateBSInfoPacket(this IPlayerEntity player, byte mode, short title, short time, short text) =>
            new BSInfoPacket
            {
                Mode = mode,
                Title = title,
                Time = time,
                Text = text
            };

        public static ChDMPacket GenerateCHDMPacket(this IPlayerEntity player, int maxhp, int angeldmg, int demondmg, int time) =>
            new ChDMPacket
            {
                Maxhp = maxhp,
                AngelDMG = angeldmg,
                DemonDMG = demondmg,
                Time = time
            };

        public static DialogPacket GenerateDialogPacket(this IPlayerEntity player, PacketBase acceptpacket, PacketBase refusepacket, string question) =>
            new DialogPacket
            {
                AcceptPacket = acceptpacket,
                RefusePacket = refusepacket,
                Question = question
            };

        public static DelayPacket GenerateDelayPacket(this IPlayerEntity player, int delay, DelayPacketType type, string argument) =>
            new DelayPacket
            {
                Delay = delay,
                Type = type,
                Argument = argument
            };
    }
}