using ChickenAPI.Data.Families;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Families.Events
{
    public class FamilyKickEvent : ChickenEventArgs
    {
        public IPlayerEntity Kicker { get; set; }
        public FamilyDto Family { get; set; }
        public string CharacterName { get; set; }
    }
}