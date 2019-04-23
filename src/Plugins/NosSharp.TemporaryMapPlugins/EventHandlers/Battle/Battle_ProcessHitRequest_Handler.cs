using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.BCard;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Battle.Events;
using ChickenAPI.Game.Battle.Extensions;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.BCards;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Monster.Events;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Npc.Events;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Events;
using ChickenAPI.Packets.Old.Game.Server.Battle;

namespace SaltyEmu.BasicPlugin.EventHandlers.Battle
{
    public class Battle_ProcessHitRequest_Handler : GenericEventPostProcessorBase<ProcessHitRequestEvent>
    {
        private readonly IBCardHandlerContainer _bCardHandlerContainer;

        public Battle_ProcessHitRequest_Handler(IBCardHandlerContainer bCardHandlerContainer)
        {
            _bCardHandlerContainer = bCardHandlerContainer;
        }

        protected override async Task Handle(ProcessHitRequestEvent e, CancellationToken cancellation)
        {
            HitRequest hitRequest = e.HitRequest;
            IBattleEntity target = hitRequest.Target;
            uint givenDamages = 0;
            await Task.Delay(hitRequest.UsedSkill.CastTime * 100, cancellation);
            List<SuPacket> packets = new List<SuPacket>();
            while (givenDamages != hitRequest.Damages && target.IsAlive)
            {
                ushort nextDamages = hitRequest.Damages - givenDamages > ushort.MaxValue ? ushort.MaxValue : (ushort)(hitRequest.Damages - givenDamages);
                givenDamages += nextDamages;
                if (target.Hp - nextDamages <= 0)
                {
                    target.Hp = 0;
                    switch (target) // send death event
                    {
                        case IPlayerEntity player:
                            await player.EmitEventAsync(new PlayerDeathEvent { Killer = hitRequest.Sender });
                            break;
                        case IMonsterEntity monster:
                            await monster.EmitEventAsync(new MonsterDeathEvent { Killer = hitRequest.Sender });
                            break;
                        case INpcEntity npc:
                            await npc.EmitEventAsync(new NpcDeathEvent { Killer = hitRequest.Sender });
                            break;
                    }

                    packets.Add(hitRequest.Sender.GenerateSuPacket(hitRequest, nextDamages));
                    break;
                }

                target.Hp -= nextDamages;
                packets.Add(hitRequest.Sender.GenerateSuPacket(hitRequest, nextDamages));
            }

            if (!packets.Any())
            {
                packets.Add(hitRequest.Sender.GenerateSuPacket(hitRequest, 0));
            }

            await hitRequest.Sender.CurrentMap.BroadcastAsync(hitRequest.Sender.GenerateEffectPacket(hitRequest.UsedSkill.CastEffect));
            await hitRequest.Sender.CurrentMap.BroadcastAsync<SuPacket>(packets);

            Log.Debug($"[{hitRequest.Sender.Type.ToString()}][{hitRequest.Sender.Id}] ATTACK -> [{hitRequest.Target.Type.ToString()}]({hitRequest.Target.Id}) : {givenDamages} damages");

            // sets the new target (for AI)
            if (hitRequest.Target.Type != VisualType.Player && !hitRequest.Target.HasTarget)
            {
                hitRequest.Target.Target = hitRequest.Sender;
            }

            foreach (BCardDto bCardDto in e.HitRequest.Bcards)
            {
                await _bCardHandlerContainer.Handle(e.HitRequest.Sender, e.HitRequest.Target, bCardDto);
            }
        }
    }
}