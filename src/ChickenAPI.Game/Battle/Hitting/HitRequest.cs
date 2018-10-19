﻿using System.Collections.Generic;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Battle.Hitting
{
    public class HitRequest
    {
        public IEntity Sender { get; set; }
        public IEntity Target { get; set; }

        public SkillDto UsedSkill { get; set; }

        public uint Damages { get; set; }

        public SuPacketHitMode HitMode { get; set; }

        // todo to bcard
        public IEnumerable<long> BuffsToApply { get; set; }

        // todo to bcard
        public IEnumerable<long> DebuffsToApply { get; set; }
    }
}