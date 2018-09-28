using System;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Game.Data.TransferObjects.Skills;

namespace ChickenAPI.Game.Data.TransferObjects.Character
{
    public class CharacterSkillDto : ISynchronizedDto
    {
        public Guid Id { get; set; }
        public long CharacterId { get; set; }

        public SkillDto Skill { get; set; }
        public long SkillId { get; set; }
    }
}