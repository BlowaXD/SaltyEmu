using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Game.Data.TransferObjects.Skills;

namespace ChickenAPI.Game.Features.Skills.Args
{
    public class SkillCastArgs : SystemEventArgs
    {
        public SkillDto Skill { get; set; }
    }
}