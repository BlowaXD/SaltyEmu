using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Game.Data.TransferObjects.Skills;

namespace ChickenAPI.Game.Data.TransferObjects.Shop
{
    public class ShopSkillDto : IMappedDto
    {
        public long ShopId { get; set; }

        public long SkillId { get; set; }

        public byte Slot { get; set; }

        public byte Type { get; set; }

        public SkillDto Skill { get; set; }
        public long Id { get; set; }
    }
}