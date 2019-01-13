using ChickenAPI.Enums.Game.Character;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Damage
{
    public class CriticalHitRateAlgorithm : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _criticalHitRate;

        public void Initialize()
        {
            _criticalHitRate = new int[(int)CharacterClassType.Unknown, MAX_LEVEL];

            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _criticalHitRate[(int)CharacterClassType.Adventurer, i] = 0; // sure
                _criticalHitRate[(int)CharacterClassType.Swordman, i] = 0; // approx
                _criticalHitRate[(int)CharacterClassType.Magician, i] = 0; // sure
                _criticalHitRate[(int)CharacterClassType.Archer, i] = 0; // sure
                _criticalHitRate[(int)CharacterClassType.Wrestler, i] = 0; // sure
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _criticalHitRate[(int)type, level - 1 > 0 ? level - 1 : 0];
    }
}