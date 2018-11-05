using System.Linq;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Battle.Extensions;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Skills.Args;
using ChickenAPI.Packets.Game.Client.Battle;
using ChickenAPI.Packets.Game.Server.Battle;

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

            if (!player.SkillComponent.Skills.TryGetValue(packet.CastId, out SkillDto skill))
            {
                // skill does not exist
                return;
            }

            IEntity target = null;
            switch (packet.TargetVisualType)
            {
                case VisualType.Character:
                    target = player.CurrentMap.GetEntitiesByType<IPlayerEntity>(VisualType.Character).FirstOrDefault(s => s.Character.Id == packet.TargetId);
                    break;
                case VisualType.Monster:
                    target = player.CurrentMap.GetEntitiesByType<IMonsterEntity>(VisualType.Monster).FirstOrDefault(s => s.MapMonster.Id == packet.TargetId);
                    break;
                case VisualType.Npc:
                    target = player.CurrentMap.GetEntitiesByType<INpcEntity>(VisualType.Npc).FirstOrDefault(s => s.MapNpc.Id == packet.TargetId);
                    break;
            }

            switch (target)
            {
                case null:
                    player.SendPacket(player.GenerateEmptyCancelPacket(CancelPacketType.InCombatMode));
                    return;
                case IBattleEntity battleEntity:
                    player.EmitEvent(new UseSkillArgs
                    {
                        Skill = skill,
                        Target = battleEntity
                    });
                    break;
            }
        }
    }
}