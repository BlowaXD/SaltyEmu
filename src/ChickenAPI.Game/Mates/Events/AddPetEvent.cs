using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class AddPetEvent : GameEntityEvent
    {
        public int MonsterVnum { get; set; }

        public MateType MateType { get; set; }
    }
}