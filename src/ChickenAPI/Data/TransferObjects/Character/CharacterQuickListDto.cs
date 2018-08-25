using System;
using ChickenAPI.Core.Data.TransferObjects;

namespace ChickenAPI.Game.Data.TransferObjects.Character
{
    public class CharacterQuicklistDto : ISynchronizedDto
    {
        public long CharacterId { get; set; }

        public bool IsSkill { get; set; }
        public short RelatedSlot { get; set; }
        public short Position { get; set; }

        public short EnumType { get; set; }
        public bool IsQ1 { get; set; }
        public Guid Id { get; set; }
    }
}