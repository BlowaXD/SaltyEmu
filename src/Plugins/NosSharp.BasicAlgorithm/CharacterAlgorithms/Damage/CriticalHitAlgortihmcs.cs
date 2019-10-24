using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Damage
{
    public class CriticalHitAlgorithm : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _criticalHit;

        public void Initialize()
        {
            _criticalHit = new int[(int)CharacterClassType.MartialArtist + 1, MAX_LEVEL];

            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _criticalHit[(int)CharacterClassType.Adventurer, i] = 0; // sure
                _criticalHit[(int)CharacterClassType.Swordman, i] = 0; // approx
                _criticalHit[(int)CharacterClassType.Magician, i] = 0; // sure
                _criticalHit[(int)CharacterClassType.Archer, i] = 0; // sure
                _criticalHit[(int)CharacterClassType.MartialArtist, i] = 0; // sure
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _criticalHit[(int)type, level - 1 > 0 ? level - 1 : 0];
    }
}