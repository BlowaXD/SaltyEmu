using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Data.AccessLayer.Skill;
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
using ChickenAPI.Game.Skills.Extensions;
using ChickenAPI.Game.Specialists;
using ChickenAPI.Game.Specialists.Extensions;

namespace ChickenAPI.Game.Features.Specialists
{
    public class SpecialistEventHandler : EventHandlerBase
    {
        private static readonly ISkillService _skillService = new Lazy<ISkillService>(() => ChickenContainer.Instance.Resolve<ISkillService>()).Value;
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


            player.SetMorph(player.Sp.Item.Morph);
            player.SendPacket(player.GenerateLevPacket());
            // set last transform
            player.Broadcast(player.GenerateCModePacket());
            player.Broadcast(player.GenerateEffectPacket(196));
            // guri packet
            player.SendPacket(player.GenerateSpPacket());
            // remove buffs
            // transform
            player.SendPacket(player.GenerateLevPacket());
            player.SendPacket(player.GenerateCondPacket());
            player.SendPacket(player.GenerateStatPacket());
            player.SendPacket(player.GenerateStatCharPacket());

            // LoadSpSkills()
            // todo find why 31
            SkillDto[] skills = _skillService.GetByClassId((byte)(player.MorphId + 31));
            player.AddSkills(skills);


            player.SendPacket(player.GenerateSkiPacket());
            player.SendPackets(player.GenerateQuicklistPacket());
            // WingsBuff
            // LoadPassive
        }

        private static void ChangePoints(IEntity entity, SpChangePointsEvent spChangePoints)
        {
        }
    }
}