using ChickenAPI.ECS.Entities;
using ChickenAPI.ECS.Systems;
using ChickenAPI.Game.Features.Skills.Args;

namespace ChickenAPI.Game.Features.Skills
{
    public class SkillSystem : NotifiableSystemBase
    {
        public SkillSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            switch (e)
            {
                case SkillCastArgs skillcast:
                    break;
                case PlayerAddSkillEventArgs addSkill:
                    break;
            }
        }
    }
}