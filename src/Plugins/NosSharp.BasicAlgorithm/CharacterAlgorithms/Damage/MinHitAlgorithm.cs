using ChickenAPI.Enums.Game.Character;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Damage
{
    public class MinHitAlgorithm : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _stats;

        public void Initialize()
        {
            _stats = new int[(int)CharacterClassType.Unknown, MAX_LEVEL];

            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _stats[(int)CharacterClassType.Adventurer, i] = i + 9; // approx
                _stats[(int)CharacterClassType.Swordman, i] = (2 * i) + 5; // approx Numbers n such that 10n+9 is prime.
                _stats[(int)CharacterClassType.Magician, i] = (2 * i) + 9; // approx Numbers n such that n^2 is of form x^ 2 + 40y ^ 2 with positive x,y.
                _stats[(int)CharacterClassType.Archer, i] = 9 + (i * 3); // approx
                _stats[(int)CharacterClassType.Wrestler, i] = 9 + (i * 3); // approx
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _stats[(int)type, level - 1 > 0 ? level - 1 : 0];
    }
}