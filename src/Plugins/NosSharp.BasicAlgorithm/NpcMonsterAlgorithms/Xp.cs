using ChickenAPI.Enums.Game.Entity;

namespace SaltyEmu.BasicAlgorithmPlugin.NpcMonsterAlgorithms
{
    public class Xp : IMonsterRaceStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[] _stats;

        public void Initialize()
        {
            _stats = new int[MAX_LEVEL];

            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _stats[i] = i < 18 ? i * 60 : i * 70;
            }
        }

        public int GetStat(NpcMonsterRaceType type, byte level, bool isMonster) => _stats[level];
    }
}