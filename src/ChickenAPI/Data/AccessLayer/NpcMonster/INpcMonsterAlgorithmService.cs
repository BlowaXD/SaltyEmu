using ChickenAPI.Enums.Game.Entity;

namespace ChickenAPI.Data.AccessLayer.NpcMonster
{
    public interface INpcMonsterAlgorithmService
    {
        /// <summary>
        /// Returns the MaxHp based on basic algorithm + special races stats
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <param name="isMonster"></param>
        /// <returns></returns>
        int GetHpMax(NpcMonsterRaceType type, byte level, bool isMonster);


        /// <summary>
        /// Returns the MaxMp based on basic algorithm + special races stats
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <param name="isMonster"></param>
        /// <returns></returns>
        int GetMpMax(NpcMonsterRaceType type, byte level, bool isMonster);

        /// <summary>
        /// Returns the Xp given by monster
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <param name="isMonster"></param>
        /// <returns></returns>
        int GetXp(NpcMonsterRaceType type, byte level, bool isMonster);

        /// <summary>
        /// Returns the JobXp given by a monster + special bonus
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <param name="isMonster"></param>
        /// <returns></returns>
        int GetJobXp(NpcMonsterRaceType type, byte level, bool isMonster);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <param name="isMonster"></param>
        /// <returns></returns>
        int GetHeroXp(NpcMonsterRaceType type, byte level, bool isMonster);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <param name="isMonster"></param>
        /// <returns></returns>
        int GetReputation(NpcMonsterRaceType type, byte level, bool isMonster);
    }
}