using System.Threading.Tasks;
using ChickenAPI.Data.BCard;
using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.BCards
{
    public interface IBCardHandlerContainer
    {
        void Register(IBCardEffectHandler handler);


        /// <summary>
        ///     Handles the entity
        /// </summary>
        /// <param name="target"></param>
        /// <param name="sender">Can be null</param>
        /// <param name="bcard"></param>
        Task Handle(IBattleEntity target, IBattleEntity sender, BCardDto bcard);
    }
}