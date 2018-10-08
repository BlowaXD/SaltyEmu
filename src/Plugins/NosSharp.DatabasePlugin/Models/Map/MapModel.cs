using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data;
using SaltyEmu.DatabasePlugin.Models.Drops;

namespace SaltyEmu.DatabasePlugin.Models.Map
{
    [Table("_data_map")]
    public class MapModel : IMappedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public string Name { get; set; }
        public bool AllowShop { get; set; }
        public bool AllowPvp { get; set; }
        public int Music { get; set; }
        public short Height { get; set; }
        public short Width { get; set; }
        public byte[] Grid { get; set; }

        public ICollection<MapMonsterModel> Monsters { get; set; }

        public ICollection<MapPortalModel> Portals { get; set; }

        public ICollection<MapDropModel> Drops { get; set; }

        public IEnumerable<MapNpcModel> Npcs { get; set; }
    }
}