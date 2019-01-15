using ChickenAPI.Data.Families;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Families.Events
{
    public class FamilyKickEvent : GameEntityEvent
    {
        public IPlayerEntity Kicker { get; set; }
        public FamilyDto Family { get; set; }
        public string CharacterName { get; set; }
    }
}