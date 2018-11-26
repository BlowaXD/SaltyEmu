using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.Player.Extension;
using ChickenAPI.Game.Skills.Extensions;
using ChickenAPI.Game.Specialists.Args;
using ChickenAPI.Game.Specialists.Extensions;

namespace ChickenAPI.Game.Specialists
{
    public class SpecialistEventHandler : EventHandlerBase
    {
        private static readonly ISkillService SkillService = new Lazy<ISkillService>(() => ChickenContainer.Instance.Resolve<ISkillService>()).Value;
        private static readonly Logger Log = Logger.GetLogger<SpecialistEventHandler>();

        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(SpTransformEvent),
            typeof(SpChangePointsEvent)
        };

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
                Log.Info("[SP_TRANSFORM] Cooldown needs to be clean");
                // should have no cooldowns
                return;
            }

            if (!player.HasSpWeared)
            {
                Log.Info("[SP_TRANSFORM] You need to wear a SpecialistCard");
                return;
            }

            // check vehicle

            if (player.IsTransformedSp)
            {
                Log.Info("[SP_TRANSFORM] You are already transformed but it's not yet implemented");
                // remove sp
                return;
            }


            // check last sp usage + sp cooldown

            ItemInstanceDto fairy = player.Inventory.GetWeared(EquipmentType.Fairy);

            if (player.Sp.ElementType != ElementType.Neutral && fairy.ElementType != player.Sp.ElementType)
            {
                return;
            }


            player.SetMorph(player.Sp.Item.Morph);
            player.SendPacket(player.GenerateLevPacket());
            // set last transform
            player.Broadcast(player.GenerateCModePacket());
            player.Broadcast(player.GenerateEffectPacket(196));
            // guri packet
            player.ActualiseUiSpPoints();
            // remove buffs
            // transform
            player.SendPacket(player.GenerateLevPacket());
            player.SendPacket(player.GenerateCondPacket());
            player.SendPacket(player.GenerateStatPacket());
            player.SendPacket(player.GenerateStatCharPacket());

            // LoadSpSkills()
            SkillDto[] skills = SkillService.GetByClassId(player.GetClassId());
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