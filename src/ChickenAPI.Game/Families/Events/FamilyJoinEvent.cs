using ChickenAPI.Data.Families;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Events;
using ChickenAPI.Packets.Enumerations;

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