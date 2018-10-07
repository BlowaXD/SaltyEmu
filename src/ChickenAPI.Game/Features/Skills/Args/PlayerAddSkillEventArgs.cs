using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Skills.Args
{
    public class PlayerAddSkillEventArgs : ChickenEventArgs
    {
        public SkillDto Skill { get; set; }

        public bool ForceChecks { get; set; } = true;
    }
}