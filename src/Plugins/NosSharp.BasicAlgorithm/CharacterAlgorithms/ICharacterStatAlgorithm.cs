using ChickenAPI.Enums.Game.Character;

namespace SaltyEmu.BasicAlgorithmPlugin.CharacterAlgorithms
{
    public interface ICharacterStatAlgorithm
    {
        void Initialize();

        int GetStat(CharacterClassType type, byte level);
    }
}