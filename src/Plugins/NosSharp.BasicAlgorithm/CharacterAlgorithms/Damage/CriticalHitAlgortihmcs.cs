using ChickenAPI.Enums.Game.Character;

namespace NosSharp.BasicAlgorithm.CharacterAlgorithms.Close
{
    public class CriticalHitAlgorithm : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _criticalHit;

        public void Initialize()
        {
            _criticalHit = new int[(int)CharacterClassType.Unknown, MAX_LEVEL];

            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _criticalHit[(int)CharacterClassType.Adventurer, i] = 0; // sure
                _criticalHit[(int)CharacterClassType.Swordman, i] = 0; // approx
                _criticalHit[(int)CharacterClassType.Magician, i] = 0; // sure
                _criticalHit[(int)CharacterClassType.Archer, i] = 0; // sure
                _criticalHit[(int)CharacterClassType.Wrestler, i] = 0; // sure
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _criticalHit[(int)type, level];
    }
}