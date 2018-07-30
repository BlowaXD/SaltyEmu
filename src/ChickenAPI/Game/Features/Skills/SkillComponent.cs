using System;
using System.Collections.Generic;
using ChickenAPI.Data.TransferObjects.Skills;

namespace ChickenAPI.Game.Features.Skills
{
    public class SkillComponent
    {
        public Dictionary<long, SkillDto> Skills { get; }
        public Queue<(DateTime, long)> CooldownsBySkillId { get; }
    }
}