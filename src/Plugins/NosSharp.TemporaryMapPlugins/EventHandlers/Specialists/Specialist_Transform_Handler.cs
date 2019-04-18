using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.Skills.Extensions;
using ChickenAPI.Game.Specialists.Args;
using ChickenAPI.Game.Specialists.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Specialists
{
    public class Specialist_Transform_Handler : GenericEventPostProcessorBase<SpTransformEvent>
    {
        private readonly ISkillService _skillService;

        public Specialist_Transform_Handler(ILogger log, ISkillService skillService) : base(log)
        {
            _skillService = skillService;
        }

        protected override async Task Handle(SpTransformEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            if (!player.HasSpWeared)
            {
                Log.Info("[SP_TRANSFORM] You need to wear a SpecialistCard");
                return;
            }

            if (player.IsTransformedLocomotion)
            {
                Log.Info("[SP_TRANSFORM] Remove your locomotion");
                return;
            }

            if (player.IsTransformedSp)
            {
                player.LastMorphUtc = DateTime.UtcNow;
                await player.EmitEventAsync(new SpRemoveEvent());
                return;
            }

            TimeSpan timeSpanSinceLastMorph = DateTime.UtcNow - player.LastMorphUtc;
            if (timeSpanSinceLastMorph <= TimeSpan.FromSeconds(player.SpCoolDown))
            {
                Log.Info($"[SP_TRANSFORM] Cooldown SP needs to be clean {player.SpCoolDown}");
                return;
            }

            if (player.CooldownsBySkillId.Any())
            {
                Log.Info("[SP_TRANSFORM] Cooldown needs to be clean");
                // should have no cooldowns
                return;
            }

            if (e.Wait)
            {
                await player.GenerateDelay(5000, DelayPacketType.Locomotion, $"#sl^0");
                return;
            }

            ItemInstanceDto fairy = player.Inventory.GetWeared(EquipmentType.Fairy);

            if (player.Sp.ElementType != ElementType.Neutral && fairy.ElementType != player.Sp.ElementType)
            {
                return;
            }

            player.SetMorph(player.Sp.Item.Morph);
            await player.SendPacketAsync(player.GenerateLevPacket());
            await player.BroadcastAsync(player.GenerateCModePacket());
            await player.BroadcastAsync(player.GenerateEffectPacket(196));
            await player.SendGuri(GuriPacketType.Unknow2, 1, (int)player.Id);
            await player.ActualiseUiSpPoints();
            // remove buffs
            // transform
            await player.SendPacketAsync(player.GenerateLevPacket());
            await player.SendPacketAsync(player.GenerateCondPacket());
            await player.SendPacketAsync(player.GenerateStatPacket());
            await player.ActualizeUiStatChar();

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