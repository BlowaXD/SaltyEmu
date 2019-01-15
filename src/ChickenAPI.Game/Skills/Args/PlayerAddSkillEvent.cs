using ChickenAPI.Data.Skills;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Skills.Args
{
    public class PlayerAddSkillEvent : GameEntityEvent
    {
        public SkillDto Skill { get; set; }

        public bool ForceChecks { get; set; } = true;
    }
}