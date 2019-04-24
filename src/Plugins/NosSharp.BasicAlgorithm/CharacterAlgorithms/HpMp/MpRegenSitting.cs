using System;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.HpMp
{
    public class MpRegenSitting : ICharacterStatAlgorithm
    {
        public void Initialize()
        {
        }

        public int GetStat(CharacterClassType type, byte level)
        {
            switch (type)
            {
                case CharacterClassType.Adventurer:
                    return 10;
                case CharacterClassType.Swordman:
                    return 30;
                case CharacterClassType.Archer:
                    return 50;
                case CharacterClassType.Magician:
                    return 80;
                case CharacterClassType.MartialArtist:
                    return 80;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}