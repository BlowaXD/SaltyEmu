using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Enums.Game.Character;
using System;

namespace ChickenAPI.Game.Data.TransferObjects.Character
{
    public class CharacterQuicklistDto : ISynchronizedDto
    {
        public Guid Id { get; set; }

        public long CharacterId { get; set; }
        
        public bool IsSkill { get; set; }
        public short RelatedSlot { get; set; }
        public short Position { get; set; }

        public short EnumType { get; set; }
        public bool IsQ1 { get; set; }
    }
}