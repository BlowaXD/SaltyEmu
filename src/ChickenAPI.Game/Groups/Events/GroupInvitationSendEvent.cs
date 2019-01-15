using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Groups.Events
{
    public class GroupInvitationSendEvent : GameEntityEvent
    {
        public IPlayerEntity Target { get; set; }
    }
}