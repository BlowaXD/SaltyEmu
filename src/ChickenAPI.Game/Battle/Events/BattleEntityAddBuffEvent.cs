using ChickenAPI.Data.BCard;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Battle.Events
{
    public class BattleEntityAddBuffEvent : ChickenEventArgs
    {
        public CardDto Card { get; set; }
    }
}