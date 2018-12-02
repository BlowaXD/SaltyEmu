using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Groups.Args
{
    public class GroupInvitationSendEvent : ChickenEventArgs
    {
        public IPlayerEntity Target { get; set; }
    }
}