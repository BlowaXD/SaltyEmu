using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Enums.Game.Entity;
using NosSharp.DatabasePlugin.Models.NpcMonster;
using NosSharp.DatabasePlugin.Models.Shop;

namespace NosSharp.DatabasePlugin.Models.Map
{
    [Table("map_npcs")]
    public class MapNpcModel : IMappedDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

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
    }
}