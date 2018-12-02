using ChickenAPI.Enums.Game.Character;

namespace NosSharp.BasicAlgorithm.CharacterAlgorithms.Close
{
    public class CriticalDistAlgorithm : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _criticalDist;

        public void Initialize()
        {
            _criticalDist = new int[(int)CharacterClassType.Unknown, MAX_LEVEL];

            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _criticalDist[(int)CharacterClassType.Adventurer, i] = 0; // sure
                _criticalDist[(int)CharacterClassType.Swordman, i] = 0; // approx
                _criticalDist[(int)CharacterClassType.Magician, i] = 0; // sure
                _criticalDist[(int)CharacterClassType.Archer, i] = 0; // sure
                _criticalDist[(int)CharacterClassType.Wrestler, i] = 0; // sure
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _criticalDist[(int)type, level];
    }
}