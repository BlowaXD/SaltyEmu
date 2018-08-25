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
            //     List<CharacterQuicklistDto> Quicklist { get; set; }
            // qset 0 0 0 0 0
            // qset 1 0 1 1 0
            // qset 1 0 2 3 1
            // Type Q1Slot Q2Slot DATA1 DATA2
            // Type = IsSkill
            // Q1Slot = if is QuickL 1 if is Yes = Slot
            // Q2 = Same q1
            // DATA 1 = EnumType 
            // DATA 2 = SlotEntity
            // IsSkill IsQ1 IsQ1 EnumType SlotEntity
            bool type = args.IsSkill;// , q1 = args.Q1;
            short q1 = args.Q1, q2 = args.Q2, data1 = args.Data1, data2 = args.Data2;

            if (q1 != 0)
            {
                player.Quicklist.Quicklist.Add(new CharacterQuicklistDto
                {
                    CharacterId = player.Character.Id,
                    IsSkill = type,
                    IsQ1 = true,
                    Slot = q1,
                    EnumType = data1,
                    Position = data2,
                    // Morph = player.Character.UseSp ? (short)player.Character.Morph : (short)0
                    Morph = 0
                });
            }

            if (q2 != 0)
            {
                player.Quicklist.Quicklist.Add(new CharacterQuicklistDto
                {
                    CharacterId = player.Character.Id,
                    IsSkill = type,
                    IsQ1 = false,
                    Slot = q2,
                    EnumType = data1,
                    Position = data2,
                    // Morph = player.Character.UseSp ? (short)player.Character.Morph : (short)0
                    Morph = 0
                });
            }

            // player.SendPacket($"qset {q1} {q2} {type}.{data1}.{data2}.0");
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