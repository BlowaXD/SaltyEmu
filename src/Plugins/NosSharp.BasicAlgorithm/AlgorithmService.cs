using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Data.AccessLayer.Character;
using NosSharp.BasicAlgorithm.CharacterAlgorithms;
using NosSharp.BasicAlgorithm.CharacterAlgorithms.Close;
using NosSharp.BasicAlgorithm.CharacterAlgorithms.Distance;
using NosSharp.BasicAlgorithm.CharacterAlgorithms.HpMp;
using NosSharp.BasicAlgorithm.CharacterAlgorithms.Magical;
using NosSharp.BasicAlgorithm.CharacterAlgorithms.Xp;
using NosSharp.BasicAlgorithm.FamilyAlgorithms;

namespace NosSharp.BasicAlgorithm
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
        private readonly ICharacterStatAlgorithm _minimumAttackRange;
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
            _minimumAttackRange = new AttackRangeAlgorithm();

            _hpMax = new HpMax();
            _hpRegen = new HpRegen();
            _hpRegenSitting = new HpRegenSitting();

            _mpMax = new HpMax();
            _mpRegen = new HpRegen();
            _mpRegenSitting = new HpRegenSitting();

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


            _hpMax.Initialize();
            _hpRegen.Initialize();
            _hpRegenSitting.Initialize();
            _mpMax.Initialize();
            _mpRegen.Initialize();
            _mpRegenSitting.Initialize();
        }

        public int GetLevelXp(CharacterClassType type, byte level) => (int)_levelBasedAlgorithm.Data[level];

        public int GetJobLevelXp(CharacterClassType type, byte level) =>
            (int)(type == CharacterClassType.Adventurer ? _jobLevelBasedAlgorithm.FirstJobXpData[level] : _jobLevelBasedAlgorithm.Data[level]);

        public int GetHeroLevelXp(CharacterClassType type, byte level) => (int)_heroLevelBasedAlgorithm.Data[level];

        public int GetSpLevelXp(byte level) => (int)_spLevelBasedAlgorithm.Data[level];

        public int GetFairyLevelXp(byte level) => (int)_fairyLevelBasedAlgorithm.Data[level];

        public int GetFamilyLevelXp(byte level) => (int)_familyLevelBasedAlgorithm.Data[level > _familyLevelBasedAlgorithm.Data.Length ? _familyLevelBasedAlgorithm.Data.Length - 1 : level];

        public int GetSpeed(CharacterClassType type, byte level) => _speedAlgorithm.GetStat(type, level);

        public int GetDefenceClose(CharacterClassType type, byte level) => _closeDefenceAlgorithm.GetStat(type, level);

        public int GetDefenceRange(CharacterClassType type, byte level) => _rangedDefenceAlgorithm.GetStat(type, level);

        public int GetDefenceMagic(CharacterClassType type, byte level) => _magicDefenceAlgorithm.GetStat(type, level);

        public int GetDodgeClose(CharacterClassType type, byte level) => _closeDodgeAlgorithm.GetStat(type, level);

        public int GetDodgeRanged(CharacterClassType type, byte level) => _rangedDodgeAlgorithm.GetStat(type, level);

        public int GetDodgeMagic(CharacterClassType type, byte level) => _magicDodgeAlgorithm.GetStat(type, level);

        public int GetMinimumAttackRange(CharacterClassType type, byte level) => _minimumAttackRange.GetStat(type, level);

        public int GetHpMax(CharacterClassType type, byte level) => _hpMax.GetStat(type, level);

        public int GetMpMax(CharacterClassType type, byte level) => _mpMax.GetStat(type, level);

        public int GetHpRegen(CharacterClassType type, byte level) => _hpRegen.GetStat(type, level);

        public int GetHpRegenSitting(CharacterClassType type, byte level) => _hpRegenSitting.GetStat(type, level);

        public int GetMpRegen(CharacterClassType type, byte level) => _mpRegen.GetStat(type, level);

        public int GetMpRegenSitting(CharacterClassType type, byte level) => _mpRegenSitting.GetStat(type, level);
    }
}