using System.Collections.Generic;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Families.Events
{
    public class FamilyCreationEvent : GameEntityEvent
    {
        public IPlayerEntity Leader { get; set; }
        public IEnumerable<IPlayerEntity> Assistants { get; set; }
        public string FamilyName { get; set; }
    }
}