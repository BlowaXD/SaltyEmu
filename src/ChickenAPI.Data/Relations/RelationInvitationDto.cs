using System;
using ChickenAPI.Enums.Game.Relations;

namespace ChickenAPI.Data.Relations
{
    public class RelationInvitationDto : ISynchronizedDto
    {
        public long OwnerId { get; set; }
        public long TargetId { get; set; }
        public CharacterRelationType RelationType { get; set; }
        public Guid Id { get; set; }
    }
}