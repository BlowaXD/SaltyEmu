using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data;
using SaltyEmu.Database;

namespace SaltyEmu.DatabasePlugin.Models.Character
{
    [Table("quicklist")]
    public class CharacterQuicklistModel : ISynchronizedModel
    {
        [Key]
        public Guid Id { get; set; }

        public CharacterModel Character { get; set; }

        [ForeignKey("FK_CHARACTERQUICKLIST_TO_CHARACTER")]
        public long CharacterId { get; set; }

        public short Morph { get; set; }

        public short Position { get; set; }

        public byte Q1 { get; set; }
        public byte Q2 { get; set; }

        public short Type { get; set; }

        public short Slot { get; set; }
    }
}