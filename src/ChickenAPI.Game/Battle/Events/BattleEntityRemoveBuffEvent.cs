using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Battle.Events
{
    public class BattleEntityRemoveBuffEvent : ChickenEventArgs
    {
        public CardDto Card { get; set; }
    }
}