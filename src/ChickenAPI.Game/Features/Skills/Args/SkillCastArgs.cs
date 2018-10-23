using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Skills.Args
{
    public class SkillCastArgs : ChickenEventArgs
    {
        public SkillDto Skill { get; set; }
        public IBattleEntity Target { get; set; }
    }
}