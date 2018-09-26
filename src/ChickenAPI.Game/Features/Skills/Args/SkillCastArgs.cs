using ChickenAPI.Core.Events;
using ChickenAPI.Game.Data.TransferObjects.Skills;

namespace ChickenAPI.Game.Features.Skills.Args
{
    public class SkillCastArgs : ChickenEventArgs
    {
        public SkillDto Skill { get; set; }
    }
}