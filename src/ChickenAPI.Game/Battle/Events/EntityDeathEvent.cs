using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Battle.Events
{
    public class EntityDeathEvent : GameEntityEvent
    {
        /// <summary>
        ///     Might be null
        /// </summary>
        public IBattleEntity Killer { get; set; }
    }
}