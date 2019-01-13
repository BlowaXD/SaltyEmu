using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Quicklist.Events;
using ChickenAPI.Game.Quicklist.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Quicklist
{
    public class Quicklist_AddElement_Handler : GenericEventPostProcessorBase<QuicklistAddElementEvent>
    {
        protected override async Task Handle(QuicklistAddElementEvent args, CancellationToken cancellation)
        {
            if (!(args.Sender is IPlayerEntity player))
            {
                return;
            }

            short type = args.Type;
            short q1 = args.Q1;
            short q2 = args.Q2;
            short data1 = args.Data1;
            short data2 = args.Data2;

            player.Quicklist.Quicklist.RemoveAll(n => n.Q1 == q1 && n.Q2 == q2 && (player.HasSpWeared ? n.Morph == player.MorphId : n.Morph == 0));
            var tmp = new CharacterQuicklistDto
            {
                Id = Guid.NewGuid(),
                CharacterId = player.Character.Id,
                Type = type,
                Q1 = q1,
                Q2 = q2,
                Slot = data1,
                Position = data2,
                Morph = player.HasSpWeared ? player.MorphId : (short)0
            };
            player.Quicklist.Quicklist.Add(tmp);

            player.SendPacket(player.Quicklist.GenerateQSetPacket(tmp));
        }
    }
}