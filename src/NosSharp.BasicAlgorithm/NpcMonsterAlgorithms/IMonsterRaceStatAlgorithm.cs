using ChickenAPI.Enums.Game.Entity;

namespace NosSharp.BasicAlgorithm
{
    public interface IMonsterRaceStatAlgorithm
    {
        void Initialize();

        int GetStat(NpcMonsterRaceType type, byte level, bool isMonster);
    }
}