using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Game.Features.Portals;
using ChickenAPI.Game.Features.Visibility.Args;
using ChickenAPI.Game.Packets;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Packets.Game.Client.Shops;
using ChickenAPI.Packets.Game.Server.Inventory;
using ChickenAPI.Game.Features.Quicklist.Args;
using ChickenAPI.Game.Data.TransferObjects.Character;
using System.Text;
using ChickenAPI.Packets.Game.Server.QuickList;

namespace ChickenAPI.Game.Features.Quicklist
{
    public class QuickListEventHandler : EventHandlerBase
    {
        private static readonly Logger Log = Logger.GetLogger<QuickListEventHandler>();

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case GenerateQuickListArgs quicklistevent:
                    SetQslot(entity as IPlayerEntity, quicklistevent);
                    break;
            }
        }

        private static void SetQslot(IPlayerEntity player, GenerateQuickListArgs args)
        {
            bool type = args.IsSkill;
            short q1 = args.Q1;
            short q2 = args.Q2;
            short data1 = args.Data1;
            short data2 = args.Data2;

            player.Quicklist.Quicklist.Add(new CharacterQuicklistDto
            {
                CharacterId = player.Character.Id,
                IsSkill = type,
                IsQ1 = q1 != 0,
                Slot = q1 != 0 ? q1 : q2,
                EnumType = data1,
                Position = data2,
                // Morph = player.Character.UseSp ? (short)player.Character.Morph : (short)0
                Morph = 0
            });
            
            player.SendPacket(GenerateQset(q1, q2, type, data1, data2));
        }

        public static QsetPacketReceive GenerateQset(short q1, short q2, bool type, short data1, short data2)
        {
            byte value = 0;
            switch (type)
            {
                case true:
                    value = 1;
                    break;
                case false:
                    value = 0;
                    break;
            }

            var tmp = new StringBuilder();

            tmp.Append(' ');
            tmp.Append(value);
            tmp.Append('.');
            tmp.Append(data1);
            tmp.Append('.');
            tmp.Append(data2);
            tmp.Append('.');
            tmp.Append("0");

            return new QsetPacketReceive
            {
                Q1 = q1,
                Q2 = q2,
                Data = tmp.ToString().Trim()
            };
        }
    }
}