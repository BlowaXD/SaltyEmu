using System;
using ChickenAPI.Enums.Game.Character;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.HpMp
{
    public class HpRegenSitting : ICharacterStatAlgorithm
    {
        public void Initialize()
        {
        }

        public int GetStat(CharacterClassType type, byte level)
        {
            switch (type)
            {
                case CharacterClassType.Adventurer:
                    return 30;
                case CharacterClassType.Swordman:
                    return 90;
                case CharacterClassType.Archer:
                    return 60;
                case CharacterClassType.Magician:
                    return 30;
                case CharacterClassType.Wrestler:
                    return 80;
                case CharacterClassType.Unknown:
                    return 50;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}