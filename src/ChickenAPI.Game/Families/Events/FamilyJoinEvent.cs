using ChickenAPI.Data.Families;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Families.Events
{
    public class FamilyJoinEvent : ChickenEventArgs
    {
        public FamilyDto Family { get; set; }
    }
}