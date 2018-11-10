using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Entities.Npc.Events
{
    public class NpcDeathEvent : ChickenEventArgs
    {
        public IBattleEntity killer { get; set; }
    }
}
