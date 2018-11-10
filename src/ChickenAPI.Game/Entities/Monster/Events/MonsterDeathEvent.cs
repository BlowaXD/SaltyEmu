using ChickenAPI.Game.Battle.Events;
using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.Entities.Monster.Events
{
    public class MonsterDeathEvent : EntityDeathEvent
    {
        public IBattleEntity killer { get; set; }
    }
}
