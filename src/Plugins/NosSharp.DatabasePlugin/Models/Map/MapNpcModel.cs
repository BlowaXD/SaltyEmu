using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data;
using ChickenAPI.Enums.Game.Entity;
using SaltyEmu.DatabasePlugin.Models.NpcMonster;
using SaltyEmu.DatabasePlugin.Models.Shop;

namespace SaltyEmu.DatabasePlugin.Models.Map
{
    [Table("map_npcs")]
    public class MapNpcModel : IMappedDto
    {
        public short Dialog { get; set; }

        public short Effect { get; set; }

        public short EffectDelay { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsMoving { get; set; }

        public bool IsSitting { get; set; }

        public MapModel Map { get; set; }

        [ForeignKey(nameof(MapId))]
        public long MapId { get; set; }

        public short MapX { get; set; }

        public short MapY { get; set; }

        public NpcMonsterModel NpcMonster { get; set; }

        public long NpcMonsterId { get; set; }

        public DirectionType Direction { get; set; }
        public ShopModel Shop { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
    }
}