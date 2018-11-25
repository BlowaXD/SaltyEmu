using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data;
using ChickenAPI.Enums.Game.Entity;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.NpcMonster;

namespace SaltyEmu.DatabasePlugin.Models.Map
{
    [Table("map_monsters")]
    public class MapMonsterModel : IMappedModel
    {
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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
    }
}