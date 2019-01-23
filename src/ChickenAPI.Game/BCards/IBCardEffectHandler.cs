using System.Threading.Tasks;
using ChickenAPI.Data.BCard;
using ChickenAPI.Enums.Game.BCard;
using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.BCards
{
    public interface IBCardEffectHandler
    {
        BCardType HandledType { get; }

        Task Handle(IBattleEntity target, IBattleEntity sender, BCardDto bcard);
    }
}