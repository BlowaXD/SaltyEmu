using ChickenAPI.Data.Skills;

namespace ChickenAPI.Data.Shop
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