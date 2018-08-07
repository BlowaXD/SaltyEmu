using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Enums.Game.Entity;
using NosSharp.DatabasePlugin.Models.BCard;
using NosSharp.DatabasePlugin.Models.Drops;
using NosSharp.DatabasePlugin.Models.NpcMonster;

namespace NosSharp.DatabasePlugin.Models.Map
{
    [Table("map_monsters")]
    public class MapMonsterModel : IMappedDto
    {
        public long Id { get; set; }

        public short MapX { get; set; }

        public short MapY { get; set; }

        public MapModel Map { get; set; }

        [ForeignKey(nameof(MapId))]
        public long MapId { get; set; }

        public NpcMonsterModel NpcMonster { get; set; }

        [ForeignKey(nameof(NpcMonsterId))]
        public long NpcMonsterId { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsMoving { get; set; }

        public DirectionType Position { get; set; }
    }
}