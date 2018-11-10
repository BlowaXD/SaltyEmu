using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Entities.Monster.Events
{
    public class MonsterDeathEvent : ChickenEventArgs
    {
        public IBattleEntity killer { get; set; }
    }
}
