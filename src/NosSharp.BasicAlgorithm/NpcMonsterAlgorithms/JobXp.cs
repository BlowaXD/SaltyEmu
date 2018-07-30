using ChickenAPI.Enums.Game.Entity;

namespace NosSharp.BasicAlgorithm.NpcMonsterAlgorithms
{
    public class JobXp : IMonsterRaceStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[] _stats;

        public void Initialize()
        {
            _stats = new int[MAX_LEVEL];

            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _stats[i] = 120;
            }
        }

        public int GetStat(NpcMonsterRaceType type, byte level, bool isMonster)
        {
            return _stats[level];
        }
    }
}