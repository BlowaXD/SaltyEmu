using ChickenAPI.Data.Skills;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Battle.Events
{
    public class BattleEntityAddBuffEvent : GameEntityEvent
    {
        public CardDto Card { get; set; }
    }
}