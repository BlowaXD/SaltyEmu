using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Enums.Game.Relations;

namespace ChickenAPI.Data.Relations
{
    public interface IRelationService
    {
        Task<IEnumerable<RelationDto>> GetRelationListByCharacterIdAsync(long characterId);

        Task<IEnumerable<RelationInvitationDto>> GetPendingInvitationByCharacterIdAsync(long characterId);

        Task<IEnumerable<RelationMessageDto>> GetPendingMessagesByCharacterIdAsync(long characterId);

        Task<RelationInvitationDto> RelationInviteAsync(long senderId, long targetId, CharacterRelationType relationType);
        Task RelationInviteAsync(RelationInvitationDto invitation);

        Task<List<RelationDto>> RelationInviteProcessAsync(Guid invitationId, RelationInvitationProcessType type);

        Task RelationAddAsync(IEnumerable<RelationDto> relations);
        Task RelationAddAsync(RelationDto add);
        Task RelationAddAsync(long senderId, long targetId, CharacterRelationType relationType);

        Task RelationRemoveAsync(IEnumerable<RelationDto> removes);
        Task RelationRemoveAsync(RelationDto remove);

        Task RelationSendMessageAsync(RelationMessageDto message);
        Task RelationSendMessageAsync(long senderId, long targetId, string message);
    }
}