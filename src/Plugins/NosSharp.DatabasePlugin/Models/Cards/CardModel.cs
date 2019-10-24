using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data.Enums.Game.Buffs;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.BCard;

namespace SaltyEmu.DatabasePlugin.Models.Cards
{
    [Table("_data_card")]
    public class CardModel : IMappedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public int Duration { get; set; }

        public int EffectId { get; set; }

        public byte Level { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public short TimeoutBuff { get; set; }

        public BuffType BuffType { get; set; }

        public byte TimeoutBuffChance { get; set; }

        public int Delay { get; set; }

        public byte Propability { get; set; }
        public IEnumerable<CardBCardModel> BCards { get; set; }
    }
}