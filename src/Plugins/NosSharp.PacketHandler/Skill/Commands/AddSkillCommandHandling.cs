using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Skills;
using ChickenAPI.Game.Features.Skills.Args;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Attributes;

namespace NosSharp.PacketHandler.Skill.Commands
{
    public class AddSkillCommandHandling
    {
        public static void OnAddSkillCommand(AddSkillCommandPacket packet, IPlayerEntity player)
        {
            var skillService = ChickenContainer.Instance.Resolve<ISkillService>();
            SkillDto skill = skillService.GetById(packet.SkillId);

            player.EmitEvent(new PlayerAddSkillEventArgs
            {
                Skill = skill,
                ForceChecks = false
            });
        }
    }

    [PacketHeader("$AddSkill", Authority = AuthorityType.GameMaster)]
    public class AddSkillCommandPacket : PacketBase
    {
        [PacketIndex(0)]
        public long SkillId { get; set; }
    }
}