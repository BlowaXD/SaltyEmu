using System;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Locomotion.Events;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.Specialists.Args;
using ChickenAPI.Game.Specialists.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Specialists
{
    public class Specialist_Remove_Handler : GenericEventPostProcessorBase<SpRemoveEvent>
    {
        private readonly IAlgorithmService _algorithm;

        public Specialist_Remove_Handler(IAlgorithmService algorithm)
        {
            _algorithm = algorithm;
        }

        protected override async Task Handle(SpRemoveEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            // Session.Character.DisableBuffs(BuffType.All);
            // Session.Character.EquipmentBCards.RemoveAll(s => s.ItemVNum.Equals(vnum));
            // CharacterHelper.RemoveSpecialistWingsBuff(Session);
            player.Speed = (byte)_algorithm.GetSpeed(player.Character.Class, player.Level);
            await player.BroadcastAsync(player.GenerateCondPacket());
            await player.BroadcastAsync(player.GenerateLevPacket());
            player.SpCoolDown = 30;

            // Tcheck Skill used
            player.SetMorph(0);
            await player.SendSdAsync(player.SpCoolDown);
            await player.BroadcastAsync(player.GenerateCModePacket());
            await player.SendGuri(GuriPacketType.Unknow2, 1, (int)player.Id);

            // ms_c
            await player.ActualizeUiSkillList();
            await player.ActualizeUiQuicklist();
            await player.SendPacketAsync(player.GenerateStatCharPacket());
            await player.SendPacketAsync(player.GenerateStatPacket());

            // Create news Task or this thread are Blocked ↑
            await Task.Factory.StartNew(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(player.SpCoolDown));
                await player.SendSdAsync(0);
                player.SpCoolDown = 0;
                await player.SendPacketAsync(player.GenerateSayPacket("TRANSFORM_DISAPPEAR", SayColorType.Purple));
            });
        }
    }
}