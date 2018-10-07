using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Skills.Args
{
    public class UseSkillArgs : ChickenEventArgs
    {
        public SkillDto Skill { get; set; }
    }
}
