using ChickenAPI.Enums.Game.Character;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Damage
{
    public class CriticalDistRateAlgorithm : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _criticalDistRate;

        public void Initialize()
        {
            _criticalDistRate = new int[(int)CharacterClassType.Unknown, MAX_LEVEL];

            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _criticalDistRate[(int)CharacterClassType.Adventurer, i] = 0; // sure
                _criticalDistRate[(int)CharacterClassType.Swordman, i] = 0; // approx
                _criticalDistRate[(int)CharacterClassType.Magician, i] = 0; // sure
                _criticalDistRate[(int)CharacterClassType.Archer, i] = 0; // sure
                _criticalDistRate[(int)CharacterClassType.Wrestler, i] = 0; // sure
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _criticalDistRate[(int)type, level - 1 > 0 ? level - 1 : 0];
    }
}