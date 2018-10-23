using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.Battle.Hitting
{
    public class BasicHitRequestFactory : IHitRequestFactory
    {
        public HitRequest CreateHitRequest(IBattleEntity sender, IBattleEntity target, SkillDto dto)
        {
            return new HitRequest
            {
                Sender = sender,
                Target = target,
                UsedSkill = dto,
                Damages = 100
            };
        }
    }
}