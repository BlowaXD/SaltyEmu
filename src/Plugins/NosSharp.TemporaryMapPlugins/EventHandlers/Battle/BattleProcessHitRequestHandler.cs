using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Battle.Events;
using ChickenAPI.Game.Battle.Extensions;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Monster.Events;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Npc.Events;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Events;
using ChickenAPI.Packets.Game.Server.Battle;

namespace SaltyEmu.BasicPlugin.EventHandlers.Battle
{
    public class BattleProcessHitRequestHandler : GenericEventPostProcessorBase<ProcessHitRequestEvent>
    {
        protected override async Task Handle(ProcessHitRequestEvent e, CancellationToken cancellation)
        {
            HitRequest hitRequest = e.HitRequest;
            IBattleEntity target = hitRequest.Target;
            uint givenDamages = 0;
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

            hitRequest.Sender.CurrentMap.Broadcast<SuPacket>(packets);
            Log.Debug($"[{hitRequest.Sender.Type.ToString()}][{hitRequest.Sender.Id}] ATTACK -> [{hitRequest.Target.Type.ToString()}]({hitRequest.Target.Id}) : {givenDamages} damages");
            // apply buffs
            // apply debuffs
        }
    }
}