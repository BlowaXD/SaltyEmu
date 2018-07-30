using ChickenAPI.Enums.Game.Character;

namespace NosSharp.BasicAlgorithm
{
    public interface ICharacterStatAlgorithm
    {
        void Initialize();

        int GetStat(CharacterClassType type, byte level);
    }
}