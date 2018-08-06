using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.ECS.Systems.Args;
using ChickenAPI.Game.Data.TransferObjects.Skills;

namespace ChickenAPI.Game.Features.Skills.Args
{
    public class PlayerAddSkillEventArgs : SystemEventArgs
    {
        public SkillDto Skill { get; set; }

        public bool ForceChecks { get; set; } = true;
    }
}