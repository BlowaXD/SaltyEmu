using ChickenAPI.Core.Data.TransferObjects;
using System;

namespace ChickenAPI.Game.Data.TransferObjects.Character
{
    public class CharacterQuicklistDto : ISynchronizedDto
    {
        public Guid Id { get; set; }

        public long CharacterId { get; set; }

        public short Morph { get; set; }

        public short Position { get; set; }

        public short Type { get; set; }
        public short Q1 { get; set; }
        public short Q2 { get; set; }

        public short Slot { get; set; }
    }
}