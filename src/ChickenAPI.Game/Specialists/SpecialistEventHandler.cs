using System;
using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Features.Effects;
using ChickenAPI.Game.Features.Skills.Extensions;
using ChickenAPI.Game.Features.Specialists.Args;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Movements.Extensions;

namespace ChickenAPI.Game.Features.Specialists
{
    public class SpecialistEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>();

        public override void Execute(IEntity entity, ChickenEventArgs args)
        {
            switch (args)
            {
                case SpTransformEvent spTransform:
                    PlayerWearSp(entity as IPlayerEntity, spTransform);
                    break;
                case SpChangePointsEvent spChangePoints:
                    ChangePoints(entity, spChangePoints);
                    break;
            }
        }

        private static void PlayerWearSp(IPlayerEntity player, SpTransformEvent spTransform)
        {
            if (player.SkillComponent.CooldownsBySkillId.Any())
            {
                // should have no cooldowns
                return;
            }

            if (!player.HasSpWeared)
            {
                return;
            }

            // check vehicle

            if (player.IsTransformedSp)
            {
                // remove sp
                return;
            }


            // check last sp usage + sp cooldown

            if (player.Inventory.GetWeared(EquipmentType.Fairy).ElementType != player.Sp.ElementType)
            {
                return;
            }

            // check fairy element
            // set last transform
            player.Broadcast(player.GenerateCModePacket());
            player.Broadcast(player.GenerateEffectPacket(196));
            // Broadcast Guri 6 1
            // remove buffs
            // transform
            player.SendPacket(player.GenerateLevPacket());
            player.SendPacket(player.GenerateCondPacket());
            player.SendPacket(player.GenerateStatPacket());
            player.SendPacket(player.GenerateStatCharPacket());

            // LoadSpSkills()
            player.SendPacket(player.GenerateSkiPacket());
            player.SendPackets(player.GenerateQuicklistPacket());
            // WingsBuff
            // LoadPassive
        }

        private static void ChangePoints(IEntity entity, SpChangePointsEvent spChangePoints)
        {
            throw new NotImplementedException();
        }
    }
}