using System.Collections.Generic;
using ChickenAPI.Data.BCard;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.Battle.Hitting
{
    public class HitRequest
    {
        public IBattleEntity Sender { get; set; }
        public IBattleEntity Target { get; set; }

        public SkillDto UsedSkill { get; set; }

        public uint Damages { get; set; }

        public SuPacketHitMode HitMode { get; set; }
        public List<BCardDto> Bcards { get; set; }
    }
}