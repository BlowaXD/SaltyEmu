using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Skills.Args
{
    public class UseSkillEvent : GameEntityEvent
    {
        public SkillDto Skill { get; set; }

        public IBattleEntity Target { get; set; }
    }
}