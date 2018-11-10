using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Battle.Events
{
    public class EntityDeathEvent : ChickenEventArgs
    {
        /// <summary>
        /// Might be null
        /// </summary>
        public IBattleEntity Killer { get; set; }
    }
}