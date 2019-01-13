using System.Collections.Generic;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Groups
{
    public interface IGroupManager
    {
        IReadOnlyCollection<GroupDto> Groups { get; }

        GroupInvitDto CreateInvitation(IPlayerEntity sender, IPlayerEntity target);

        void RemoveInvitation(GroupInvitDto dto);
        void AcceptInvitation(GroupInvitDto dto);

        void AddGroup(GroupDto group);
        void RemoveGRoup(GroupDto group);
    }
}