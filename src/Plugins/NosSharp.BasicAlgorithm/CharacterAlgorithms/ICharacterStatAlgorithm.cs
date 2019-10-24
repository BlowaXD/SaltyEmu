using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms
{
    public interface ICharacterStatAlgorithm
    {
        void Initialize();

        int GetStat(CharacterClassType type, byte level);
    }
}