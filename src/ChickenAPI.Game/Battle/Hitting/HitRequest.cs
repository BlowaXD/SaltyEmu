using System.Collections.Generic;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Battle.Hitting
{
    public class HitRequest
    {
        public IEntity Sender { get; set; }
        public IEntity Target { get; set; }

        public SkillDto UsedSkill { get; set; }

        public uint Damages { get; set; }

        // todo to bcard
        public IEnumerable<long> BuffsToApply { get; set; }

        // todo to bcard
        public IEnumerable<long> DebuffsToApply { get; set; }
    }
}