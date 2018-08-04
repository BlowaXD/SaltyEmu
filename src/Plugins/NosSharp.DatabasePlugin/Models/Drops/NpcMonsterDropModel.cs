using System.ComponentModel.DataAnnotations.Schema;
using NosSharp.DatabasePlugin.Models.NpcMonster;

namespace NosSharp.DatabasePlugin.Models.Drops
{
    [Table("_data_npc_monster_drops")]
    public class NpcMonsterDropModel : DropModel
    {

        public NpcMonsterModel NpcMonster { get; set; }

        /// <summary>
        /// This can be MapTypeId, NpcMonsterId...
        /// </summary>
        public long TypedId { get; set; }

    }
}