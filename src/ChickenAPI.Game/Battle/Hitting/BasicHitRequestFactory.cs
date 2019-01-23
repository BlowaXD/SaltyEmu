using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.BCard;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.Battle.Hitting
{
    public class BasicHitRequestFactory : IHitRequestFactory
    {
        private readonly IBCardService _bCardService;

        public BasicHitRequestFactory(IBCardService bCardService)
        {
            _bCardService = bCardService;
        }

        public async Task<HitRequest> CreateHitRequest(IBattleEntity sender, IBattleEntity target, SkillDto dto) =>
            new HitRequest
            {
                Sender = sender,
                Target = target,
                UsedSkill = dto,
                Damages = 0,
                Bcards = new List<BCardDto>(await _bCardService.GetBySkillIdAsync(dto.Id))
            };
    }
}