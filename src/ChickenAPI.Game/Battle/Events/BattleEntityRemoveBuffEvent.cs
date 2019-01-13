using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Battle.Events
{
    public class BattleEntityRemoveBuffEvent : GameEntityEvent
    {
        public CardDto Card { get; set; }
    }
}