using System.Collections.Generic;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Groups
{
    public interface IGroupManager
    {
        IReadOnlyCollection<Group> Groups { get; }

        GroupInvitDto CreateInvitation(IPlayerEntity sender, IPlayerEntity target);

        void RemoveInvitation(GroupInvitDto dto);
        void AcceptInvitation(GroupInvitDto dto);

        void AddGroup(Group group);
        void RemoveGRoup(Group group);
    }
}