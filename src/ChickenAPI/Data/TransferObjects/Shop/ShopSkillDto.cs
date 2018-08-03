using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Data.TransferObjects.Skills;

namespace ChickenAPI.Game.Data.TransferObjects.Shop
{
    public class ShopSkillDto : IMappedDto
    {
        public long Id { get; set; }

        public long ShopId { get; set; }

        public long SkillId { get; set; }

        public byte Slot { get; set; }

        public byte Type { get; set; }

        public SkillDto Skill { get; set; }
    }
}
