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

        public short PositionX { get; set; }
        public short PositionY { get; set; }


        public MapModel Map { get; set; }

        [ForeignKey("FK_MAPNPCMONSTER_TO_MAPID")]
        public long MapId { get; set; }


        public NpcMonsterModel NpcMonster { get; set; }

        [ForeignKey("FK_MAPNPCMONSTER_TO_NPCMONSTER")]
        public long NpcMonsterId { get; set; }
    }
}