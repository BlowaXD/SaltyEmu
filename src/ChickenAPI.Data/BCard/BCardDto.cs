using ChickenAPI.Enums.Game.BCard;

namespace ChickenAPI.Data.BCard
{
    public class BCardDto : IMappedDto
    {
        public byte SubType { get; set; }

        public BCardType Type { get; set; }

        public int FirstData { get; set; }

        public int SecondData { get; set; }

        public int ThirdData { get; set; }

        public byte CastType { get; set; }

        public bool IsLevelScaled { get; set; }

        public bool IsLevelDivided { get; set; }

        public BCardRelationType RelationType { get; set; }

        public long RelationId { get; set; }
        public long Id { get; set; }
    }
}