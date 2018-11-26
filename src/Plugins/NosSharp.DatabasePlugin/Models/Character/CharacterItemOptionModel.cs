using System;
using System.ComponentModel.DataAnnotations.Schema;
using SaltyEmu.Database;

namespace SaltyEmu.DatabasePlugin.Models.Character
{
    [Table("character_item_options")]
    public class CharacterItemOptionModel : ISynchronizedModel
    {
        public Guid Id { get; set; }

        public byte Level { get; set; }

        public byte Type { get; set; }

        public long Value { get; set; }

        public CharacterItemModel CharacterItem { get; set; }

        [ForeignKey(nameof(CharacterItemModel))]
        public Guid WearableInstanceId { get; set; }
    }
}