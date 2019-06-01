using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms.Magical
{
    public class MagicDodgeAlgorithm : ICharacterStatAlgorithm
    {
        public void Initialize()
        {
            // no dodge possible without shells effects
        }

        public int GetStat(CharacterClassType type, byte level) => 0;
    }
}