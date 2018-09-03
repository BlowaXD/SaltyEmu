using System.ComponentModel.DataAnnotations.Schema;
using SaltyEmu.DatabasePlugin.Models.NpcMonster;

namespace SaltyEmu.DatabasePlugin.Models.Drops
{
    [Table("_data_npc_monster_drops")]
    public class NpcMonsterDropModel : DropModel
    {
        public NpcMonsterModel NpcMonster { get; set; }

        /// <summary>
        ///     This can be MapTypeId, NpcMonsterId...
        /// </summary>
        public long TypedId { get; set; }
    }
}