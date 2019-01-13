using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;
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

namespace SaltyEmu.BasicPlugin.EventHandlers.Specialists
{
    public class Specialist_Transform_Handler : GenericEventPostProcessorBase<SpTransformEvent>
    {
        private readonly ISkillService _skillService;

        public Specialist_Transform_Handler(ISkillService skillService)
        {
            _skillService = skillService;
        }

        protected override async Task Handle(SpTransformEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

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
            await player.SendPacketAsync(player.GenerateLevPacket());
            // set last transform
            await player.BroadcastAsync(player.GenerateCModePacket());
            await player.BroadcastAsync(player.GenerateEffectPacket(196));
            // guri packet
            await player.ActualiseUiSpPoints();
            // remove buffs
            // transform
            await player.SendPacketAsync(player.GenerateLevPacket());
            await player.SendPacketAsync(player.GenerateCondPacket());
            await player.SendPacketAsync(player.GenerateStatPacket());
            await player.SendPacketAsync(player.GenerateStatCharPacket());

            // LoadSpSkills()
            SkillDto[] skills = await _skillService.GetByClassIdAsync(player.GetSpClassId());
            player.AddSkills(skills);

            await player.ActualizeUiSkillList();
            await player.ActualizeUiQuicklist();

            // WingsBuff
            // LoadPassive
        }
    }
}