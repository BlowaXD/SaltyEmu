using System;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.HpMp
{
    public class MpRegen : ICharacterStatAlgorithm
    {
        public void Initialize()
        {
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
                case CharacterClassType.MartialArtist:
                    return 50;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}