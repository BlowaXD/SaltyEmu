using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.HpMp
{
    public class HpMax : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _stats;

        public void Initialize()
        {
            _stats = new int[(int)CharacterClassType.MartialArtist + 1, MAX_LEVEL];

            // todo improve that shit
            for (int i = 0; i < MAX_LEVEL; i++)
            {
                int jSwordman = 16;
                int hpSwordman = 946;
                int incSwordman = 85;
                while (jSwordman <= i)
                {
                    if ((jSwordman % 5) == 2)
                    {
                        hpSwordman += incSwordman / 2;
                        incSwordman += 2;
                    }
                    else
                    {
                        hpSwordman += incSwordman;
                        incSwordman += 4;
                    }

                    ++jSwordman;
                }

                int hpArcher = 680;
                int incArcher = 35;
                int jArcher = 16;
                while (jArcher <= i)
                {
                    hpArcher += incArcher;
                    ++incArcher;
                    if ((jArcher % 10) == 1 || (jArcher % 10) == 5 || (jArcher % 10) == 8)
                    {
                        hpArcher += incArcher;
                        ++incArcher;
                    }

                    ++jArcher;
                }

                _stats[(int)CharacterClassType.Adventurer, i] = (int)(1 / 2.0 * i * i + 31 / 2.0 * i + 205); // approx
                _stats[(int)CharacterClassType.Swordman, i] = hpSwordman;
                _stats[(int)CharacterClassType.Magician, i] = (int)((i + 15) * (i + 15) + i + 15.0 - 465 + 550); // approx
                _stats[(int)CharacterClassType.Archer, i] = hpArcher; // approx
                _stats[(int)CharacterClassType.MartialArtist, i] = hpSwordman; // approx
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _stats[(int)type, level - 1 > 0 ? level - 1 : 0];
    }
}