using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Damage
{
    public class MaxHitAlgorithm : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _maxHit;

        public void Initialize()
        {
            _maxHit = new int[(int)CharacterClassType.MartialArtist + 1, MAX_LEVEL];

            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _maxHit[(int)CharacterClassType.Adventurer, i] = i + 9; // approx
                _maxHit[(int)CharacterClassType.Swordman, i] = (2 * i) + 5; // approx Numbers n such that 10n+9 is prime.
                _maxHit[(int)CharacterClassType.Magician, i] = (2 * i) + 9; // approx Numbers n such that n^2 is of form x^2+40y^2 with positive x,y.
                _maxHit[(int)CharacterClassType.Archer, i] = 9 + (i * 3); // approx
                _maxHit[(int)CharacterClassType.MartialArtist, i] = 9 + (i * 3); // nop
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _maxHit[(int)type, level - 1 > 0 ? level - 1 : 0];
    }
}