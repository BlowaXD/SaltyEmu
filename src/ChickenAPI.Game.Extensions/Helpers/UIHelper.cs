using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game._i18n;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.Interfaces;
using ChickenAPI.Packets.ServerPackets.UI;

namespace ChickenAPI.Game.Helpers
{
    public static class UIHelper
    {
        public static Task SendGuri(this IPlayerEntity player, GuriPacketType type, byte argument, int value = 0) => player.SendPacketAsync(player.GenerateGuriPacket(type, argument, value));

        public static GuriPacket GenerateGuriPacket(this IPlayerEntity player, GuriPacketType type, byte argument, int value = 0)
        {
            return null;
            /*
            switch (type)
            {
                case GuriPacketType.Unknow:
                    return new GuriPacket
                    {
                        Type = (GuriPacketType)2,
                        EntityId = player.Id,
                        Unknown = argument,
                        Value = (uint)value,
                    };

                case GuriPacketType.Unknow2:
                    return new GuriPacket
                    {
                        Type = 6,
                        Argument = 1,
                        VisualEntityId = player.Id,
                        Value = "0",
                        Data = 0
                    };

                case GuriPacketType.Unknow3:
                    return new GuriPacket
                    {
                        Type = 10,
                        Argument = argument,
                        VisualEntityId = value,
                        Value = player.Id.ToString()
                    };

                case GuriPacketType.Unknow4:
                    return new GuriPacket
                    {
                        Type = 15,
                        Argument = argument,
                        VisualEntityId = 0,
                        Data = 0
                    };

                default:
                    return new GuriPacket
                    {
                        Type = (int)type,
                        Argument = argument,
                        VisualEntityId = player.Id,
                        Value = value.ToString()
                    };
            }
            */
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

        public static Task SendTopscreenMessage(this IPlayerEntity player, string msg, MessageType type) => player.SendPacketAsync(player.GenerateMsgPacket(msg, type));

        public static Task GenerateBSInfo(this IPlayerEntity player, byte mode, short title, short time, short text) => player.SendPacketAsync(player.GenerateBSInfoPacket(mode, title, time, text));

        public static Task GenerateCHDM(this IPlayerEntity player, int maxhp, int angeldmg, int demondmg, int time) =>
            player.SendPacketAsync(player.GenerateCHDMPacket(maxhp, angeldmg, demondmg, time));

        public static Task SendDialog(this IPlayerEntity player, IPacket yesPacket, IPacket noPacket, string question) =>
            player.SendPacketAsync(player.GenerateDialogPacket(yesPacket, noPacket, question));

        public static Task SendQuestionAsync(this IPlayerEntity player, IPacket yesPacket, string question) => player.SendPacketAsync(player.GenerateQnaPacket(yesPacket, question));

        public static Task SendDelayAsync(this IPlayerEntity player, short delay, DelayPacketType type, IPacket argument) => player.SendPacketAsync(player.GenerateDelayPacket(delay, type, argument));

        public static QnaPacket GenerateQnaPacket(this IPlayerEntity player, IPacket yesPacket, string question) =>
            new QnaPacket
            {
                YesPacket = yesPacket,
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

        public static ChDMPacket GenerateCHDMPacket(this IPlayerEntity player, int maxHp, int angelDamage, int demonDamage, int time) =>
            new ChDMPacket
            {
                Maxhp = maxHp,
                AngelDMG = angelDamage,
                DemonDMG = demonDamage,
                Time = time
            };

        public static DlgPacket GenerateDialogPacket(this IPlayerEntity player, IPacket yesPacket, IPacket noPacket, string question) =>
            new DlgPacket
            {
                YesPacket = yesPacket,
                NoPacket = noPacket,
                Question = question
            };

        public static DelayPacket GenerateDelayPacket(this IPlayerEntity player, short delay, DelayPacketType type, IPacket packet) =>
            new DelayPacket
            {
                Delay = delay,
                Type = (byte)type,
                Packet = packet,
            };
    }
}