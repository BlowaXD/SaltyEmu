using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Enums.Game.Character;

namespace NosSharp.DatabasePlugin.Models.Character
{
    [Table("quicklist")]
    public class CharacterQuikListModel : ISynchronizedDto
    {
        [Key]
        public Guid Id { get; set; }

        public CharacterModel Character { get; set; }

        [ForeignKey("FK_CHARACTERQUICKLIST_TO_CHARACTER")]
        public long CharacterId { get; set; }

        [Required]     
        public short Morph { get; set; }

        public short Pos { get; set; }

        public short Q1 { get; set; }

        public short Q2 { get; set; }

        public short Slot { get; set; }

        public short Type { get; set; }
    }
}