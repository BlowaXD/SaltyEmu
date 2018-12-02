using System;
using System.Collections.Generic;
using System.Text;
using ChickenAPI.Data.Skills;

namespace ChickenAPI.Game.Buffs
{
    public class BuffContainer
    {
        public BuffContainer(CardDto card, int level = 0)
        {
            Card = card;
            Level = level;
            Start = DateTime.Now;
        }

        public CardDto Card { get; }

        public int Level { get; }

        public bool IsStaticBuff { get; }
        public bool IsPermaBuff { get; }

        public bool StaticBuff { get; set; }

        public int RemainingTime { get; set; }

        public DateTime Start { get; set; }
    }
}