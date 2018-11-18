using ChickenAPI.Data.Character;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Quicklist.Extensions
{
    public static class QsetGenerationExtension
    {
        public static QsetPacketReceive GenerateQSetPacket(this QuicklistComponent quicklist, CharacterQuicklistDto args)
        {
            string tmp = $"{args.Type}.{args.Slot}.{args.Position}.0";

            return new QsetPacketReceive
            {
                Q1 = args.Q1,
                Q2 = args.Q2,
                Data = tmp
            };
        }

        public static QsetPacketReceive GenerateRemoveQSetPacket(this QuicklistComponent comp, short q1, short q2)
        {
            const string tmp = "7.7-1.0";

            return new QsetPacketReceive
            {
                Q1 = q1,
                Q2 = q2,
                Data = tmp
            };
        }
    }
}