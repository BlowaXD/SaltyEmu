using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.Battle.HitRequest
{
    public class BasicHitRequestFactory : IHitRequestFactory
    {
        public Hitting.HitRequest CreateHitRequest(IBattleEntity sender, IBattleEntity target)
        {
            return new Hitting.HitRequest
            {
                Sender = sender.Battle.Entity,
                Target = target.Battle.Entity,
            };
        }
    }
}