using ChickenAPI.Enums.Game.Character;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Damage
{
    public class MaxDistanceAlgorithm : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _maxDist;

        public void Initialize()
        {
            _maxDist = new int[(int)CharacterClassType.Unknown, MAX_LEVEL];

            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _maxDist[(int)CharacterClassType.Adventurer, i] = i + 9; // approx
                _maxDist[(int)CharacterClassType.Swordman, i] = i + 12; // approx
                _maxDist[(int)CharacterClassType.Magician, i] = 14 + i; // approx
                _maxDist[(int)CharacterClassType.Archer, i] = 2 * i; // approx
                _maxDist[(int)CharacterClassType.Wrestler, i] = 2 * i; // approx
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _maxDist[(int)type, level - 1 > 0 ? level - 1 : 0];
    }
}