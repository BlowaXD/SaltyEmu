using ChickenAPI.Data.TransferObjects.Skills;
using ChickenAPI.ECS.Systems;

namespace ChickenAPI.Game.Features.Skills.Args
{
    public class PlayerAddSkillEventArgs : SystemEventArgs
    {
        public SkillDto Skill { get; set; }
    }
}
