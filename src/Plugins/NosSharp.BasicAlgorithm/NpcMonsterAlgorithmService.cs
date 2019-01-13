using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Enums.Game.Entity;
using SaltyEmu.BasicAlgorithmPlugin.NpcMonsterAlgorithms;

namespace SaltyEmu.BasicAlgorithmPlugin
{
    public class NpcMonsterAlgorithmService : INpcMonsterAlgorithmService
    {
        private readonly IMonsterRaceStatAlgorithm _heroXp;
        private readonly IMonsterRaceStatAlgorithm _hpMax;
        private readonly IMonsterRaceStatAlgorithm _jobXp;
        private readonly IMonsterRaceStatAlgorithm _mpMax;
        private readonly IMonsterRaceStatAlgorithm _xp;

        public NpcMonsterAlgorithmService()
        {
            _hpMax = new HpMax();
            _mpMax = new MpMax();
            _xp = new Xp();
            _jobXp = new JobXp();
            _heroXp = new HeroXp();

            _hpMax.Initialize();
            _mpMax.Initialize();
            _xp.Initialize();
            _jobXp.Initialize();
            _heroXp.Initialize();
        }


        public int GetHpMax(NpcMonsterRaceType type, byte level, bool isMonster) => _hpMax.GetStat(type, level, isMonster);

        public int GetMpMax(NpcMonsterRaceType type, byte level, bool isMonster) => _mpMax.GetStat(type, level, isMonster);

        public int GetXp(NpcMonsterRaceType type, byte level, bool isMonster) => _xp.GetStat(type, level, isMonster);

        public int GetJobXp(NpcMonsterRaceType type, byte level, bool isMonster) => _jobXp.GetStat(type, level, isMonster);

        public int GetHeroXp(NpcMonsterRaceType type, byte level, bool isMonster) => _heroXp.GetStat(type, level, isMonster);

        public int GetReputation(NpcMonsterRaceType type, byte level, bool isMonster) => 0;
    }
}