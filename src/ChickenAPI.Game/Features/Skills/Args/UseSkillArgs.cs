using ChickenAPI.Data.Skills;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Skills.Args
{
    public class UseSkillArgs : ChickenEventArgs
    {
        public SkillDto Skill { get; set; }

        public IEntity targetEntity { get; set; }
    }
}
