using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Enums.Game.Character;

namespace NosSharp.DatabasePlugin.Models.Character
{
    [Table("quicklist")]
    public class CharacterQuicklistModel : ISynchronizedDto
    {
        [Key]
        public Guid Id { get; set; }

        public CharacterModel Character { get; set; }

        [ForeignKey("FK_CHARACTERQUICKLIST_TO_CHARACTER")]
        public long CharacterId { get; set; }

        public bool IsSkill { get; set; }
        public short RelatedSlot { get; set; }
        public short Position { get; set; }

        public short EnumType { get; set; }
        public bool IsQ1 { get; set; }
    }
}