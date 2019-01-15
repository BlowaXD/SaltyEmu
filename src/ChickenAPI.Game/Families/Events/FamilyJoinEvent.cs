using ChickenAPI.Data.Families;
using ChickenAPI.Enums.Game.Families;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Families.Events
{
    public class FamilyJoinEvent : GameEntityEvent
    {
        public FamilyDto Family { get; set; }
        public IPlayerEntity Player { get; set; }
        public bool Force { get; set; }

        public FamilyAuthority ExpectedAuthority { get; set; } = FamilyAuthority.Member;
    }
}