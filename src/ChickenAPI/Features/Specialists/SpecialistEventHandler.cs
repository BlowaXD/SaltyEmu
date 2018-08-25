using System;
using System.Linq;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Features.Movement.Extensions;
using ChickenAPI.Game.Features.Specialists.Args;

namespace ChickenAPI.Game.Features.Specialists
{
    public class SpecialistEventHandler : EventHandlerBase
    {
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
            if (player.Skills.CooldownsBySkillId.Any())
            {
                // should have no cooldowns
                return;
            }

            if (player.Inventory.Wear[(int)EquipmentType.Sp] == null)
            {
                // should have item weared !
                return;
            }

            // check vehicle

            if (player.Sp != null)
            {
                // remove SP
                return;
            }

            // check last sp usage + sp cooldown
            // check fairy element
            // set last transform
            // Broadcast cmode()
            // Broadcast Eff 196
            // Broadcast Guri 6 1
            // remove buffs
            // transform
            player.SendPacket(player.GenerateLevPacket());
            player.SendPacket(player.GenerateCondPacket());
            player.SendPacket(player.GenerateStatPacket());
            player.SendPacket(player.GenerateStatCharPacket());

            // LoadSpSkills()
            // GenerateSki()
            // GenerateQuicklist()
            // WingsBuff
            // LoadPassive
        }

        private static void ChangePoints(IEntity entity, SpChangePointsEvent spChangePoints)
        {
            throw new NotImplementedException();
        }
    }
}