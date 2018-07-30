using System;
using ChickenAPI.Enums.Game.Character;

namespace NosSharp.BasicAlgorithm.CharacterAlgorithms
{
    public class SpeedAlgorithm : ICharacterStatAlgorithm
    {
        public void Initialize()
        {
            return;
        }

        public int GetStat(CharacterClassType type, byte level)
        {
            switch (type)
            {
                case CharacterClassType.Adventurer:
                    return 11;
                case CharacterClassType.Swordman:
                    return 11;
                case CharacterClassType.Archer:
                    return 12;
                case CharacterClassType.Magician:
                    return 10;
                case CharacterClassType.Wrestler:
                    return 11;
                case CharacterClassType.Unknown:
                    return 11;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}