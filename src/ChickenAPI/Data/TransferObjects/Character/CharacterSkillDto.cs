using System;
using ChickenAPI.Core.Data.TransferObjects;

namespace ChickenAPI.Game.Data.TransferObjects.Character
{
    public class CharacterSkillDto : ISynchronizedDto
    {
        public long CharacterId { get; set; }
        public long SkillId { get; set; }
        public Guid Id { get; set; }
    }
}