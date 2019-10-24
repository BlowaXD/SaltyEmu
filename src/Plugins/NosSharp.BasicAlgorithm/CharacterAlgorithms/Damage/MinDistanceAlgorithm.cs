using System;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Damage
{
    public class MinDistanceAlgorithm : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _minDist;

        public void Initialize()
        {
            _minDist = new int[Enum.GetValues(typeof(CharacterClassType)).Length + 1, MAX_LEVEL];

            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _minDist[(int)CharacterClassType.Adventurer, i] = i + 9; // approx
                _minDist[(int)CharacterClassType.Swordman, i] = i + 12; // approx
                _minDist[(int)CharacterClassType.Magician, i] = 14 + i; // approx
                _minDist[(int)CharacterClassType.Archer, i] = 2 * i; // approx
                _minDist[(int)CharacterClassType.MartialArtist, i] = 2 * i; // approx
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _minDist[(int)type, level - 1 > 0 ? level - 1 : 0];
    }
}