using System;
using System.Collections.Generic;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;

namespace ChickenAPI.Game.Groups
{
    public class BasicGroupManager : IGroupManager
    {
        private readonly Dictionary<long, Group> _groups = new Dictionary<long, Group>();
        private readonly Dictionary<Guid, GroupInvitDto> _pendingInvitations = new Dictionary<Guid, GroupInvitDto>();
        private readonly Dictionary<long, List<GroupInvitDto>> _pendingInvitationsByCharacterId = new Dictionary<long, List<GroupInvitDto>>();
        private long _lastGroupId;

        protected long NextGroupId => ++_lastGroupId;


        public IReadOnlyCollection<Group> Groups => _groups.Values;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns>null</returns>
        public IEnumerable<GroupInvitDto> GetPendingInvitationsByCharacterId(long characterId)
        {
            return !_pendingInvitationsByCharacterId.TryGetValue(characterId, out List<GroupInvitDto> groups) ? null : groups;
        }

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
            if (!_pendingInvitationsByCharacterId.TryGetValue(target.Id, out List<GroupInvitDto> invitations))
            {
                invitations = new List<GroupInvitDto>();
                _pendingInvitationsByCharacterId[target.Id] = invitations;
            }

            invitations.Add(invitation);

            return invitation;
        }

        public void RemoveInvitation(GroupInvitDto dto)
        {
            if (_pendingInvitations.ContainsKey(dto.Id))
            {
                if (_pendingInvitationsByCharacterId.TryGetValue(dto.Target.Id, out List<GroupInvitDto> group))
                {
                    group.Remove(dto);
                }

                _pendingInvitations.Remove(dto.Id);
            }
        }

        public void AcceptInvitation(GroupInvitDto dto)
        {
            if (!_pendingInvitations.TryGetValue(dto.Id, out dto))
            {
                return;
            }

            RemoveInvitation(dto);

            if (!dto.Sender.HasGroup && !dto.Target.HasGroup)
            {
                CreateGroup(dto.Sender);
            }

            dto.Target.JoinGroup(dto.Sender.Group);
        }

        public void AddGroup(Group group)
        {
            if (!_groups.TryGetValue(group.Id, out Group tmp))
            {
                _groups.Add(group.Id, group);
            }
        }

        public void RemoveGroup(Group group)
        {
            if (_groups.ContainsKey(group.Id))
            {
                _groups.Remove(group.Id);
            }
        }

        private void CreateGroup(IPlayerEntity leader)
        {
            leader.Group = new Group
            {
                Id = NextGroupId,
                Leader = leader,
                Players = new List<IPlayerEntity>(new[] { leader })
            };
        }
    }
}