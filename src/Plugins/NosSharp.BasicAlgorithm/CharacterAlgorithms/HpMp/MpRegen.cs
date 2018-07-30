using System;
using ChickenAPI.Enums.Game.Character;

namespace NosSharp.BasicAlgorithm.CharacterAlgorithms.HpMp
{
    public class MpRegen : ICharacterStatAlgorithm
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
                    return 5;
                case CharacterClassType.Swordman:
                    return 16;
                case CharacterClassType.Archer:
                    return 28;
                case CharacterClassType.Magician:
                    return 40;
                case CharacterClassType.Wrestler:
                    return 50;
                case CharacterClassType.Unknown:
                    return 50;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}