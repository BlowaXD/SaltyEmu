using System;
using System.Linq;
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
using ChickenAPI.Game.Features.Quicklist.Extensions;

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
                    HandleQslot(entity as IPlayerEntity, quicklistevent);
                    break;
            }
        }

        private static void HandleQslot(IPlayerEntity player, GenerateQuickListArgs args)
        {
            switch (args.Type)
            {
                case 0:
                case 1:
                    SetQslot(player, args);
                    break;
                case 2:
                    SwitchQslot(player, args);
                    break;
                case 3:
                    RemoveQslot(player, args);
                    break;
            }
        }

        private static void SetQslot(IPlayerEntity player, GenerateQuickListArgs args)
        {
            short type = args.Type;
            short q1 = args.Q1;
            short q2 = args.Q2;
            short data1 = args.Data1;
            short data2 = args.Data2;

            // player.Quicklist.Quicklist.RemoveAll(n => n.Q1 == q1 && n.Q2 == q2 && (player.Character.UseSp ? n.Morph == player.Character.Morph : n.Morph == 0));
            var tmp = new CharacterQuicklistDto
            {
                Id = Guid.NewGuid(),
                CharacterId = player.Character.Id,
                Type = type,
                Q1 = q1,
                Q2 = q2,
                Slot = data1,
                Position = data2,
                // Morph = player.Character.UseSp ? (short)player.Character.Morph : (short)0
                Morph = 0
            };
            player.Quicklist.Quicklist.Add(tmp);

            player.SendPacket(player.Quicklist.GenerateQSetPacket(tmp));
        }

        private static void RemoveQslot(IPlayerEntity player, GenerateQuickListArgs args)
        {
            CharacterQuicklistDto qlFrom = player.Quicklist.Quicklist.FirstOrDefault(n => n.Q1 == args.Data1 && n.Q2 == args.Data2);

            if (qlFrom == null)
            {
                // can't remove what does not exist
                return;
            }

            player.Quicklist.Quicklist.Remove(qlFrom);
            player.SendPacket(player.Quicklist.GenerateRemoveQSetPacket(qlFrom.Q1, qlFrom.Q2));
        }

        private static void SwitchQslot(IPlayerEntity player, GenerateQuickListArgs args)
        {
            // check sp
            CharacterQuicklistDto qlFrom = player.Quicklist.Quicklist.FirstOrDefault(n => n.Q1 == args.Data1 && n.Q2 == args.Data2);

            if (qlFrom == null)
            {
                // modified packet
                return;
            }

            // check sp
            CharacterQuicklistDto qlTo = player.Quicklist.Quicklist.FirstOrDefault(s => s.Q1 == args.Q1 && s.Q2 == args.Q2);

            if (qlTo == null)
            {
                player.SendPacket(player.Quicklist.GenerateRemoveQSetPacket(qlFrom.Q1, qlFrom.Q2));
            }
            else
            {
                qlTo.Q1 = qlFrom.Q1;
                qlTo.Q1 = qlFrom.Q2;
            }


            qlFrom.Q1 = args.Data1;
            qlFrom.Q2 = args.Data2;


            player.SendPacket(player.Quicklist.GenerateQSetPacket(qlFrom));
        }
    }
}