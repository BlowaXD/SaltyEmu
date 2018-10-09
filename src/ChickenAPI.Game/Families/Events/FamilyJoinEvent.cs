using ChickenAPI.Data.Families;
using ChickenAPI.Enums.Game.Families;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Families.Events
{
    public class FamilyJoinEvent : ChickenEventArgs
    {
        public FamilyDto Family { get; set; }
        public bool Force { get; set; }

        public FamilyAuthority ExpectedAuthority { get; set; }
    }
}