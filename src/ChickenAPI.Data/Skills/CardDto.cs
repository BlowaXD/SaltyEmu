﻿using ChickenAPI.Data.Enums.Game.Buffs;

namespace ChickenAPI.Data.Skills
{
    public class CardDto : IMappedDto
    {
        public int Duration { get; set; }

        public int EffectId { get; set; }

        public byte Level { get; set; }

        public string Name { get; set; }

        public short TimeoutBuff { get; set; }

        public BuffType BuffType { get; set; }

        public byte TimeoutBuffChance { get; set; }

        public int Delay { get; set; }

        public byte Propability { get; set; }
        public long Id { get; set; }
    }
}