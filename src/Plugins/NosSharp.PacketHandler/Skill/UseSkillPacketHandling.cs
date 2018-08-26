using System.Linq;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities;
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

            IEntity target = null;
            switch (packet.TargetVisualType)
            {
                case VisualType.Monster:
                    target = player.EntityManager.GetEntitiesByType<IMonsterEntity>(EntityType.Monster).FirstOrDefault(s => s.Id == packet.TargetId);
                    break;
                case VisualType.Npc:
                    target = player.EntityManager.GetEntitiesByType<INpcEntity>(EntityType.Monster).FirstOrDefault(s => s.Id == packet.TargetId);
                    break;
            }

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
                AttackAnimation = 0,
                SkillCooldown = 0,
                SkillEffect = 0,
                SkillVnum = 0,
                SkillTypeMinusOne = 0
            });

        }
    }
}