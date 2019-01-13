using ChickenAPI.Enums.Game.Entity;

namespace SaltyEmu.BasicAlgorithmPlugin.NpcMonsterAlgorithms
{
    public class HeroXp : IMonsterRaceStatAlgorithm
    {
        private const int MAX_LEVEL = 256;

        public void Initialize()
        {
        }

        public int GetStat(NpcMonsterRaceType type, byte level, bool isMonster) => 0;
    }
}