using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Character;
using SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms;
using SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Close;
using SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Damage;
using SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Distance;
using SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.HpMp;
using SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Magical;
using SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Xp;
using SaltyEmu.BasicAlgorithmPlugin.FamilyAlgorithms;

namespace SaltyEmu.BasicAlgorithmPlugin
{
    public class AlgorithmService : IAlgorithmService
    {
        private readonly ICharacterStatAlgorithm _closeDefenceAlgorithm;
        private readonly ICharacterStatAlgorithm _closeDodgeAlgorithm;
        private readonly ILevelBasedDataAlgorithm _fairyLevelBasedAlgorithm;
        private readonly ILevelBasedDataAlgorithm _familyLevelBasedAlgorithm;
        private readonly ILevelBasedDataAlgorithm _heroLevelBasedAlgorithm;

        private readonly ICharacterStatAlgorithm _hpMax;
        private readonly ICharacterStatAlgorithm _hpRegen;
        private readonly ICharacterStatAlgorithm _hpRegenSitting;
        private readonly JobLevelBasedAlgorithm _jobLevelBasedAlgorithm;
        private readonly ILevelBasedDataAlgorithm _levelBasedAlgorithm;
        private readonly ICharacterStatAlgorithm _magicDefenceAlgorithm;
        private readonly ICharacterStatAlgorithm _magicDodgeAlgorithm;
        private readonly ICharacterStatAlgorithm _minHit;
        private readonly ICharacterStatAlgorithm _maxHit;
        private readonly ICharacterStatAlgorithm _minDist;
        private readonly ICharacterStatAlgorithm _maxDist;
        private readonly ICharacterStatAlgorithm _hitRate;
        private readonly ICharacterStatAlgorithm _criticalHitRate;
        private readonly ICharacterStatAlgorithm _criticalHit;
        private readonly ICharacterStatAlgorithm _criticalDistRate;
        private readonly ICharacterStatAlgorithm _criticalDist;
        private readonly ICharacterStatAlgorithm _mpMax;
        private readonly ICharacterStatAlgorithm _mpRegen;
        private readonly ICharacterStatAlgorithm _mpRegenSitting;
        private readonly ICharacterStatAlgorithm _rangedDefenceAlgorithm;
        private readonly ICharacterStatAlgorithm _rangedDodgeAlgorithm;

        private readonly ICharacterStatAlgorithm _speedAlgorithm;
        private readonly ILevelBasedDataAlgorithm _spLevelBasedAlgorithm;

        public AlgorithmService()
        {
            _levelBasedAlgorithm = new LevelBasedAlgorithm();
            _jobLevelBasedAlgorithm = new JobLevelBasedAlgorithm();
            _spLevelBasedAlgorithm = new SpLevelBasedAlgorithm();
            _heroLevelBasedAlgorithm = new HeroLevelBasedAlgorithm();
            _fairyLevelBasedAlgorithm = new FairyLevelBasedAlgorithm();
            _familyLevelBasedAlgorithm = new FamilyLevelBasedAlgorithm();

            _speedAlgorithm = new SpeedAlgorithm();

            _closeDefenceAlgorithm = new CloseDefenceAlgorithm();
            _rangedDefenceAlgorithm = new RangedDefenceAlgorithm();
            _magicDefenceAlgorithm = new MagicDefenceAlgorithm();

            _closeDodgeAlgorithm = new CloseDodgeAlgorithm();
            _rangedDodgeAlgorithm = new RangedDodgeAlgorithm();
            _magicDodgeAlgorithm = new MagicDodgeAlgorithm();

            _minHit = new MinHitAlgorithm();
            _maxHit = new MaxHitAlgorithm();
            _minDist = new MinDistanceAlgorithm();
            _maxDist = new MaxDistanceAlgorithm();
            _hitRate = new HitRateAlgorithm();
            _criticalHitRate = new CriticalHitRateAlgorithm();
            _criticalHit = new CriticalHitAlgorithm();
            _criticalDistRate = new CriticalDistRateAlgorithm();
            _criticalDist = new CriticalDistAlgorithm();

            _hpMax = new HpMax();
            _hpRegen = new HpRegen();
            _hpRegenSitting = new HpRegenSitting();

            _mpMax = new MpMax();
            _mpRegen = new MpRegen();
            _mpRegenSitting = new MpRegenSitting();

            _levelBasedAlgorithm.Initialize();
            _jobLevelBasedAlgorithm.Initialize();
            _spLevelBasedAlgorithm.Initialize();
            _heroLevelBasedAlgorithm.Initialize();
            _fairyLevelBasedAlgorithm.Initialize();
            _familyLevelBasedAlgorithm.Initialize();

            _closeDefenceAlgorithm.Initialize();
            _rangedDefenceAlgorithm.Initialize();
            _magicDefenceAlgorithm.Initialize();

            _closeDodgeAlgorithm.Initialize();
            _rangedDodgeAlgorithm.Initialize();
            _magicDodgeAlgorithm.Initialize();

            _minHit.Initialize();
            _maxHit.Initialize();
            _minDist.Initialize();
            _maxDist.Initialize();
            _hitRate.Initialize();
            _criticalHitRate.Initialize();
            _criticalHit.Initialize();
            _criticalDistRate.Initialize();
            _criticalDist.Initialize();

            _hpMax.Initialize();
            _hpRegen.Initialize();
            _hpRegenSitting.Initialize();
            _mpMax.Initialize();
            _mpRegen.Initialize();
            _mpRegenSitting.Initialize();
        }

        public int GetDistCritical(CharacterClassType type, byte level) => _criticalDist.GetStat(type, level);

        public int GetDistCriticalRate(CharacterClassType type, byte level) => _criticalDistRate.GetStat(type, level);

        public int GetHitCritical(CharacterClassType type, byte level) => _criticalHit.GetStat(type, level);

        public int GetHitCriticalRate(CharacterClassType type, byte level) => _criticalHitRate.GetStat(type, level);

        public int GetHitRate(CharacterClassType type, byte level) => _hitRate.GetStat(type, level);

        public int GetMaxDistance(CharacterClassType type, byte level) => _maxDist.GetStat(type, level);

        public int GetMaxHit(CharacterClassType type, byte level) => _maxHit.GetStat(type, level);

        public int GetMinHit(CharacterClassType type, byte level) => _minHit.GetStat(type, level);

        public long GetLevelXp(CharacterClassType type, byte level) => _levelBasedAlgorithm.Data[level - 1 > 0 ? level - 1 : 0];

        public int GetJobLevelXp(CharacterClassType type, byte level) =>
            (int)(type == CharacterClassType.Adventurer ? _jobLevelBasedAlgorithm.FirstJobXpData[level - 1 > 0 ? level - 1 : 0] : _jobLevelBasedAlgorithm.Data[level - 1 > 0 ? level - 1 : 0]);

        public int GetHeroLevelXp(CharacterClassType type, byte level) => (int)_heroLevelBasedAlgorithm.Data[level - 1 > 0 ? level - 1 : 0];

        public int GetSpLevelXp(byte level) => (int)_spLevelBasedAlgorithm.Data[level - 1];

        public int GetFairyLevelXp(byte level) => (int)_fairyLevelBasedAlgorithm.Data[level - 1];

        public int GetFamilyLevelXp(byte level) => (int)_familyLevelBasedAlgorithm.Data[level > _familyLevelBasedAlgorithm.Data.Length ? _familyLevelBasedAlgorithm.Data.Length - 1 : level - 1];

        public int GetSpeed(CharacterClassType type, byte level) => _speedAlgorithm.GetStat(type, level);

        public int GetDefenceClose(CharacterClassType type, byte level) => _closeDefenceAlgorithm.GetStat(type, level);

        public int GetDefenceRange(CharacterClassType type, byte level) => _rangedDefenceAlgorithm.GetStat(type, level);

        public int GetDefenceMagic(CharacterClassType type, byte level) => _magicDefenceAlgorithm.GetStat(type, level);

        public int GetDodgeClose(CharacterClassType type, byte level) => _closeDodgeAlgorithm.GetStat(type, level);

        public int GetDodgeRanged(CharacterClassType type, byte level) => _rangedDodgeAlgorithm.GetStat(type, level);

        public int GetDodgeMagic(CharacterClassType type, byte level) => _magicDodgeAlgorithm.GetStat(type, level);

        public int GetMinimumAttackRange(CharacterClassType type, byte level) => _minDist.GetStat(type, level);

        public int GetHpMax(CharacterClassType type, byte level) => _hpMax.GetStat(type, level);

        public int GetMpMax(CharacterClassType type, byte level) => _mpMax.GetStat(type, level);

        public int GetHpRegen(CharacterClassType type, byte level) => _hpRegen.GetStat(type, level);

        public int GetHpRegenSitting(CharacterClassType type, byte level) => _hpRegenSitting.GetStat(type, level);

        public int GetMpRegen(CharacterClassType type, byte level) => _mpRegen.GetStat(type, level);

        public int GetMpRegenSitting(CharacterClassType type, byte level) => _mpRegenSitting.GetStat(type, level);
    }
}