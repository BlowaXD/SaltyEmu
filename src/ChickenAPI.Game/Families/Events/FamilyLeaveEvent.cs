using ChickenAPI.Data.Families;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Families.Events
{
    public class FamilyLeaveEvent : GameEntityEvent
    {
        public IPlayerEntity Player { get; set; }
        public FamilyDto Family { get; set; }
    }
}