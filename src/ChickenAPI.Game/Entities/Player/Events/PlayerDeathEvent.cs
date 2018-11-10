using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Entities.Player.Events
{
    public class PlayerDeathEvent : ChickenEventArgs
    {
        public IBattleEntity killer { get; set; }
    }
}
