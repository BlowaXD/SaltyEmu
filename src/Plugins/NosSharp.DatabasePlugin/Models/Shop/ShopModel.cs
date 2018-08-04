using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.Data.TransferObjects;
using NosSharp.DatabasePlugin.Models.Map;

namespace NosSharp.DatabasePlugin.Models.Shop
{
    [Table("map_npcs_shop")]
    public class ShopModel : IMappedDto
    {
        [Key]
        public long Id { get; set; }

        public MapNpcModel MapNpc { get; set; }

        [ForeignKey(nameof(MapNpcId))]
        public long MapNpcId { get; set; }

        public string Name { get; set; }

        public byte MenuType { get; set; }

        public byte ShopType { get; set; }

        public IEnumerable<ShopItemModel> ShopItems { get; set; }

        public IEnumerable<RecipeModel> Recipes { get; set; }
        public IEnumerable<ShopSkillModel> ShopSkills { get; set; }
    }
}