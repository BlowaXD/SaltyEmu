using ChickenAPI.Data.Families;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Families.Events
{
    public class FamilyLeaveEvent : ChickenEventArgs
    {
        public IPlayerEntity Player { get; set; }
        public FamilyDto Family { get; set; }
    }
}