using ChickenAPI.Enums.Game.Entity;

namespace NosSharp.BasicAlgorithm.NpcMonsterAlgorithms
{
    public class HpMax : IMonsterRaceStatAlgorithm
    {
        private const int MAX_LEVEL = 256;
        private int[] _stats;

        public void Initialize()
        {
            _stats = new int[MAX_LEVEL];


            // basicHpLoad
            int baseHp = 138;
            int hpBaseSup = 18;
            for (int i = 0; i < MAX_LEVEL; i++)
            {
                _stats[i] = baseHp;
                hpBaseSup++;
                baseHp += hpBaseSup;

                if (i == 37)
                {
                    baseHp = 1765;
                    hpBaseSup = 65;
                }

                if (i < 41)
                {
                    continue;
                }

                if (((99 - i) % 8) == 0)
                {
                    hpBaseSup++;
                }
            }
        }

        public int GetStat(NpcMonsterRaceType type, byte level, bool isMonster)
        {
            int hpSupp = 0;
            switch (type)
            {
                case NpcMonsterRaceType.Race0UnknownYet:
                    return RaceZeroStats(level, isMonster);
            }

            return _stats[level] + hpSupp;
        }

        private int RaceZeroStats(byte level, bool isMonster)
        {
            double hp = 1;
            hp = 138;
            int hpUp = 17;
            for (int i = 0; i < level; i++)
            {
                hpUp++;
                hp += hpUp;
            }

            if (!isMonster)
            {
                return (int)hp;
            }

            if (level <= 71)
            {
                if (level > 36)
                {
                    hp += hp / 5.0;
                }

                if (level > 51)
                {
                    hp += hp / 4.0;
                }

                if (level > 61)
                {
                    hp += hp / 5.0;
                }
            }
            else if (level > 71 && level <= 81)
            {
                hp += hp * 1.5;
            }
            else if (level > 81)
            {
                hp += hp * 2.5;
            }

            return (int)hp;
        }
    }
}