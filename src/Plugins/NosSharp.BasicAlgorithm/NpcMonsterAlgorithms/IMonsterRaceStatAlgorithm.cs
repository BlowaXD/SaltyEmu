using ChickenAPI.Data.Enums.Game.Entity;

namespace SaltyEmu.BasicAlgorithmPlugin.NpcMonsterAlgorithms
{
    public interface IMonsterRaceStatAlgorithm
    {
        void Initialize();

        int GetStat(NpcMonsterRaceType type, byte level, bool isMonster);
    }
}