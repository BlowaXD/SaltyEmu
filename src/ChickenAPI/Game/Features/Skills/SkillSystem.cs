using ChickenAPI.ECS.Entities;
using ChickenAPI.ECS.Systems;

namespace ChickenAPI.Game.Features.Skills
{
    public class SkillSystem : NotifiableSystemBase
    {
        public SkillSystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}