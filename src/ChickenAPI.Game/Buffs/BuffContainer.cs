using System;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Buffs;

namespace ChickenAPI.Game.Buffs
{
    public class BuffContainer
    {
        public BuffContainer(CardDto card, int level = 0, bool isPermaBuff = false, bool isStaticBuff = false)
        {
            Card = card;
            Level = level;
            IsPermaBuff = isPermaBuff;
            IsStaticBuff = isStaticBuff;
            Start = DateTime.Now;
        }

        public long Id => Card.Id;
        public BuffType BuffType => Card.BuffType;
        public long Duration => Card.Duration;

        public long EffectId => Card.EffectId;

        public long BuffToAddOnTimeout => Card.TimeoutBuff;
        public byte BuffToAddOnTimeoutChance => Card.TimeoutBuffChance;

        private CardDto Card { get; }

        public int Level { get; }

        public bool IsStaticBuff { get; }
        public bool IsPermaBuff { get; }

        public DateTime Start { get; }
    }
}