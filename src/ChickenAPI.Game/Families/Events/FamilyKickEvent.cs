using ChickenAPI.Data.Families;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Families.Events
{
    public class FamilyKickEvent : ChickenEventArgs
    {
        public FamilyDto Family { get; set; }
        public long CharacterId { get; set; }
        public string CharacterName { get; set; }
    }
}