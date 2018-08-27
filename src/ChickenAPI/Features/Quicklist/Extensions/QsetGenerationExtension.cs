using System.Linq;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Quicklist.Args;
using ChickenAPI.Packets.Game.Server.QuickList;

namespace ChickenAPI.Game.Features.Quicklist.Extensions
{
    public static class QsetGenerationExtension
    {
        public static QsetPacketReceive GenerateQset(GenerateQuickListArgs args)
        {
            string tmp = $"{args.Type}.{args.Data1}.{args.Data2}.0";

            return new QsetPacketReceive
            {
                Q1 = args.Q1,
                Q2 = args.Q2,
                Data = tmp
            };
        }

        public static QsetPacketReceive RemoveQset(GenerateQuickListArgs args)
        {
            string tmp = $"7.7-1.0";

            return new QsetPacketReceive
            {
                Q1 = args.Q1,
                Q2 = args.Q2,
                Data = tmp
            };
        }

        public static QsetPacketReceive SwitchQset(IPlayerEntity player, GenerateQuickListArgs args)
        {
            /*CharacterQuicklistDto qlFrom =
                      player.Quicklist.Quicklist.FirstOrDefault(n => n.Q1 == data1 && n.Q2 == data2 && (Session.Character.UseSp ? n.Morph == Session.Character.Morph : n.Morph == 0));
*/
            CharacterQuicklistDto qlFrom =
                      player.Quicklist.Quicklist.FirstOrDefault(n => n.Q1 == args.Data1 && n.Q2 == args.Data2);

            if (qlFrom != null)
            {
                //QuicklistEntryDTO qlTo =
                //Session.Character.QuicklistEntries.FirstOrDefault(n => n.Q1 == q1 && n.Q2 == q2 && (Session.Character.UseSp ? n.Morph == Session.Character.Morph : n.Morph == 0));
                CharacterQuicklistDto qlTo =
          player.Quicklist.Quicklist.FirstOrDefault(n => n.Q1 == args.Q1 && n.Q2 == args.Q2);

                qlFrom.Q1 = args.Q1;
                qlFrom.Q2 = args.Q2;
                if (qlTo == null)
                {
                    // Put 'from' to new position (datax)
                    string Data = $"{qlFrom.Type}.{qlFrom.Slot}.{qlFrom.Position}.0";

                    player.SendPacket(new QsetPacketReceive
                    {
                        Q1 = qlFrom.Q1,
                        Q2 = qlFrom.Q2,
                        Data = Data
                    });

                    // old 'from' is now empty.
                    string tmp = $"7.7-1.0";

                    return new QsetPacketReceive
                    {
                        Q1 = args.Data1,
                        Q2 = args.Data2,
                        Data = tmp
                    };
                }
                else
                {
                    // Put 'from' to new position (datax)
                    string Data = $"{qlFrom.Type}.{qlFrom.Slot}.{qlFrom.Position}.0";

                    player.SendPacket(new QsetPacketReceive
                    {
                        Q1 = qlFrom.Q1,
                        Q2 = qlFrom.Q2,
                        Data = Data
                    });

                    // 'from' is now 'to' because they exchanged
                    qlTo.Q1 = args.Data1;
                    qlTo.Q2 = args.Data2;

                    string tmp = $"{qlTo.Type}.{qlTo.Slot}.{qlTo.Position}.0";

                    return new QsetPacketReceive
                    {
                        Q1 = qlTo.Q1,
                        Q2 = qlTo.Q2,
                        Data = tmp
                    };
                }
            }
            else
            {
                return new QsetPacketReceive { };
            }
            
        }
    }
}