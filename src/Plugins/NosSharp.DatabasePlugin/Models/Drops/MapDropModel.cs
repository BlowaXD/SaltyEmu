using System.ComponentModel.DataAnnotations.Schema;
using NosSharp.DatabasePlugin.Models.Map;

namespace NosSharp.DatabasePlugin.Models.Drops
{
    [Table("map_drop")]
    public class MapDropModel : DropModel
    {
        /// <summary>
        /// </summary>
        public MapModel Map { get; set; }

        /// <summary>
        ///     This can be MapTypeId, NpcMonsterId...
        /// </summary>
        [ForeignKey("FK_MAPDROP_TO_MAP")]
        public long TypedId { get; set; }
    }
}