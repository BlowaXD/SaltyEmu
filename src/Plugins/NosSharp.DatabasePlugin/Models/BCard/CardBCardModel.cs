using System.ComponentModel.DataAnnotations.Schema;
using SaltyEmu.DatabasePlugin.Models.Cards;

namespace SaltyEmu.DatabasePlugin.Models.BCard
{
    [Table("_data_card_bcard")]
    public class CardBCardModel : BCardModel
    {
        public CardModel Card { get; set; }

        [Column("CardId")]
        [ForeignKey("FK_CARDBCARD_TO_CARD")]
        public long RelationId { get; set; }
    }
}