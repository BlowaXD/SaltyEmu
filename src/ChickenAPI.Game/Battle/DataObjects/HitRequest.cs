using System.Collections.Generic;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Data.TransferObjects.Skills;

namespace ChickenAPI.Game.Battle.DataObjects
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