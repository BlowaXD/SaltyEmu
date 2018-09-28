using System.Linq;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client._NotYetSorted;
using ChickenAPI.Packets.Game.Server.Battle;
using ChickenAPI.Packets.Game.Server.QuickList.Battle;

namespace NosSharp.PacketHandler.Skill
{
    public class UseSkillPacketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<UseSkillPacketHandling>();

        public static void OnUseSkillPacket(UseSkillPacket packet, IPlayerEntity player)
        {
            Log.Warn($"{packet.OriginalContent}");

            if (packet.CastId != 0)
            {
                player.SendPacket(new MscPacket());
            }

            if (player.Movable.IsSitting)
            {
                player.Movable.IsSitting = false;
            }

            IEntity target = null;
            switch (packet.TargetVisualType)
            {
                case VisualType.Character:
                    target = player.EntityManager.GetEntitiesByType<IPlayerEntity>(VisualType.Character).FirstOrDefault(s => s.Character.Id == packet.TargetId);
                    break;
                case VisualType.Monster:
                    target = player.EntityManager.GetEntitiesByType<IMonsterEntity>(VisualType.Monster).FirstOrDefault(s => s.MapMonster.Id == packet.TargetId);
                    break;
                case VisualType.Npc:
                    target = player.EntityManager.GetEntitiesByType<INpcEntity>(VisualType.Npc).FirstOrDefault(s => s.MapNpc.Id == packet.TargetId);
                    break;
            }

            SkillDto tmp = player.Skills.Skills.Values.FirstOrDefault(s => s.CastId == packet.CastId);

            if (target == null)
            {
                player.SendPacket(new CancelPacket
                {
                    Type = CancelPacketType.NotInCombatMode,
                    TargetId = 0,
                });
                return;
            }

            player.SendPacket(new SuPacket
            {
                VisualType = VisualType.Character,
                VisualId = player.Character.Id,
                HitMode = SuPacketHitMode.SuccessAttack,
                Damage = 100,
                HpPercentage = 100,
                PositionX = player.Movable.Actual.X,
                PositionY = player.Movable.Actual.Y,
                TargetId = packet.TargetId,
                TargetVisualType = packet.TargetVisualType,
                TargetIsAlive = true,
                AttackAnimation = tmp?.AttackAnimation ?? 0,
                SkillCooldown = tmp?.Cooldown ?? 0,
                SkillEffect = tmp?.Effect ?? 0,
                SkillVnum = tmp?.Id ?? 0,
                SkillTypeMinusOne = tmp?.SkillType - 1 ?? 0
            });
        }
    }
}