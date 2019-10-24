using System;
using ChickenAPI.Packets.Enumerations;

namespace ChickenAPI.Data.Relations
{
    public class RelationDto : ISynchronizedDto
    {
        public CharacterRelationType Type { get; set; }
        public long OwnerId { get; set; }
        public long TargetId { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}