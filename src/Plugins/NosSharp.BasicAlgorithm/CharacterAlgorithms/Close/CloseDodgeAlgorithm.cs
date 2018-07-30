using ChickenAPI.Enums.Game.Character;

namespace NosSharp.BasicAlgorithm.CharacterAlgorithms.Close
{
    public class CloseDodgeAlgorithm : ICharacterStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[,] _stats;

        public void Initialize()
        {
            _stats = new int[(int)CharacterClassType.Unknown, MAX_LEVEL];


            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _stats[(int)CharacterClassType.Adventurer, i] = i + 9; // approx
                _stats[(int)CharacterClassType.Swordman, i] = i + 12; // approx
                _stats[(int)CharacterClassType.Magician, i] = i + 24; // approx
                _stats[(int)CharacterClassType.Archer, i] = i + 41; // approx
                _stats[(int)CharacterClassType.Wrestler, i] = i + 12; // approx
            }
        }

        public int GetStat(CharacterClassType type, byte level) => _stats[(int)type, level];
    }
}