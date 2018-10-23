using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Battle.Events
{
    public class TargetHitRequest : ChickenEventArgs
    {
        public IBattleEntity Target { get; set; }
        public SkillDto Skill { get; set; }
    }
}
