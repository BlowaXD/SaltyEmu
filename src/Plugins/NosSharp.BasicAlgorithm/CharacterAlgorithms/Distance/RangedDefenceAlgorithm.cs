using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Distance
{
    public class RangedDefenceAlgorithm : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _stats;

        public void Initialize()
        {
            _stats = new int[(int)CharacterClassType.MartialArtist + 1, MAX_LEVEL];


            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _stats[(int)CharacterClassType.Adventurer, i] = (i + 9) / 2; // approx
                _stats[(int)CharacterClassType.Swordman, i] = i; // approx
                _stats[(int)CharacterClassType.Magician, i] = i + 20; // approx
                _stats[(int)CharacterClassType.Archer, i] = i; // approx
                _stats[(int)CharacterClassType.MartialArtist, i] = i; // approx
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _stats[(int)type, level - 1 > 0 ? level - 1 : 0];
    }
}