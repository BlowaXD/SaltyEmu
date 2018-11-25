using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Managers;

namespace ChickenAPI.Game.Groups
{
    public class BasicGroupManager : IGroupManager
    {
        private readonly IPlayerManager PlayerManager = new Lazy<IPlayerManager>(() => ChickenContainer.Instance.Resolve<IPlayerManager>()).Value;
        private Dictionary<Guid, GroupInvitDto> _pendingInvitations = new Dictionary<Guid, GroupInvitDto>();
        private Dictionary<long, GroupDto> _groups = new Dictionary<long, GroupDto>();


        public IReadOnlyCollection<GroupDto> Groups => _groups.Values;

        public GroupInvitDto CreateInvitation(IPlayerEntity sender, IPlayerEntity target)
        {
            var invitation = new GroupInvitDto
            {
                Id = Guid.NewGuid(),
                Sender = sender,
                Target = target,
                CreationUtc = DateTime.UtcNow
            };
            _pendingInvitations.Add(invitation.Id, invitation);
            return invitation;
        }

        public void RemoveInvitation(GroupInvitDto dto)
        {
            if (_pendingInvitations.ContainsKey(dto.Id))
            {
                _pendingInvitations.Remove(dto.Id);
            }
        }

        public void AcceptInvitation(GroupInvitDto dto)
        {
            if (!_pendingInvitations.TryGetValue(dto.Id, out dto))
            {
                return;
            }
        }

        private void CreateGroup(IPlayerEntity leader)
        {

        }

        public void AddGroup(GroupDto @group)
        {
            if (!_groups.TryGetValue(@group.Id, out GroupDto tmp))
            {
                _groups.Add(group.Id, group);
            }
        }

        public void RemoveGRoup(GroupDto @group)
        {
            if (_groups.ContainsKey(@group.Id))
            {
                _groups.Remove(group.Id);
            }
        }
    }
}