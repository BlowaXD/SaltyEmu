using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.HpMp
{
    public class MpMax : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _stats;

        public void Initialize()
        {
            _stats = new int[(int)CharacterClassType.MartialArtist + 1, MAX_LEVEL];

            // todo improve that shit
            int actual = 60;
            int baseAdventurer = 9;
            for (int i = 0; i < MAX_LEVEL; i++)
            {
                if ((i % 3) == 0)
                {
                    baseAdventurer++;
                }

                if ((i % 4) == 0)
                {
                    baseAdventurer++;
                }

                actual += baseAdventurer;

                _stats[(int)CharacterClassType.Adventurer, i] = actual; // approx
                _stats[(int)CharacterClassType.Swordman, i] = actual;
                _stats[(int)CharacterClassType.Magician, i] = 3 * actual; // approx
                _stats[(int)CharacterClassType.Archer, i] = actual + baseAdventurer; // approx
                _stats[(int)CharacterClassType.MartialArtist, i] = actual; // approx
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _stats[(int)type, level - 1 > 0 ? level - 1 : 0];
    }
}