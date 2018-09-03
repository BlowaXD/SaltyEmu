using System.ComponentModel.DataAnnotations.Schema;
using SaltyEmu.DatabasePlugin.Models.NpcMonster;

namespace SaltyEmu.DatabasePlugin.Models.BCard
{
    [Table("_data_npc_monster_bcard")]
    public class NpcMonsterBCardModel : BCardModel
    {
        public NpcMonsterModel NpcMonster { get; set; }

        [Column("NpcMonsterId")]
        [ForeignKey("FK_NPCMONSTERBCARD_TO_NPCMONSTER")]
        public long RelationId { get; set; }
    }
}