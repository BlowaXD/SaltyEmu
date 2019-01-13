using ChickenAPI.Data.Skills;

namespace ChickenAPI.Game.Buffs
{
    public class BuffBuilder
    {
        private CardDto _card;

        public BuffBuilder WithCard(CardDto dto)
        {
            _card = dto;
            return this;
        }

        public BuffContainer Build() => new BuffContainer(_card);

        public static implicit operator BuffContainer(BuffBuilder builder) => builder.Build();
    }
}