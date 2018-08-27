using System;
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

            player.Quicklist.Quicklist.Add(new CharacterQuicklistDto
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
            });

            player.SendPacket(QsetGenerationExtension.GenerateQset(args));

        }

        private static void RemoveQslot(IPlayerEntity player, GenerateQuickListArgs args)
        {

            //  player.Quicklist.Quicklist.RemoveAll(n => n.Q1 == args.Q1 && n.Q2 == args.Q2 && (Session.Character.UseSp ? n.Morph == Session.Character.Morph : n.Morph == 0));

            player.SendPacket(QsetGenerationExtension.RemoveQset(args));
        }

        private static void SwitchQslot(IPlayerEntity player, GenerateQuickListArgs args)
        {
            player.SendPacket(QsetGenerationExtension.SwitchQset(player, args));
        }
    }
}