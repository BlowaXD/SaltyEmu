using System;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms
{
    public class SpeedAlgorithm : ICharacterStatAlgorithm
    {
        public void Initialize()
        {
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
                case CharacterClassType.MartialArtist:
                    return 11;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}