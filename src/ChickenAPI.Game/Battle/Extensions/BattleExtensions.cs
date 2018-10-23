using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Packets.Game.Server.Battle;

namespace ChickenAPI.Game.Battle.Extensions
{
    public static class BattleExtensions
    {
        private static readonly IHitRequestFactory HitRequestFactory = new Lazy<IHitRequestFactory>(() => ChickenContainer.Instance.Resolve<IHitRequestFactory>()).Value;

        public static HitRequest CreateHitRequest(this IBattleEntity entity, IBattleEntity target, SkillDto skill)
        {
            HitRequest request = HitRequestFactory.CreateHitRequest(entity, target, skill);

            return request;
        }

        public static void ProcessHitRequest(this IBattleEntity entity, HitRequest hit)
        {
            if (hit.Target != entity)
            {
                // this should never be here
                return;
            }

            BattleComponent battle = entity.Battle;
            uint damage = hit.Damages;
            List<SuPacket> packets = new List<SuPacket>();
            while (damage > 0)
            {
                hit.Damages = damage > ushort.MaxValue ? ushort.MaxValue : (ushort)damage;
                damage -= hit.Damages;
                packets.Add(entity.GenerateSuPacket(hit));
                if (battle.Hp - hit.Damages <= 0)
                {
                    battle.Hp = 0;
                    // Generate Death
                    break;
                }

                battle.Hp -= (ushort)hit.Damages;
            }

            if (entity.CurrentMap is IMapLayer broadcastable)
            {
                broadcastable.Broadcast<SuPacket>(packets);
            }

            // apply buffs
            // apply debuffs
        }
    }
}