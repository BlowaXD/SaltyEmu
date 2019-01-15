using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Mates.Events
{
    public class AddPetEvent : GameEntityEvent
    {
        public int MonsterVnum { get; set; }

        public MateType MateType { get; set; }
    }
}