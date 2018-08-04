using ChickenAPI.Enums.Game.Character;

namespace ChickenAPI.Game.Data.AccessLayer.Character
{
    /// <summary>
    ///     IAlgorithmService should provide the methods to recover some algorithm based datas of the game such as Xp, JobXp...
    /// </summary>
    public interface IAlgorithmService
    {
        #region Xp

        /// <summary>
        ///     This method will search through algorithm service and return the LevelXp stat based on
        ///     <see cref="CharacterClassType" /> and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetLevelXp(CharacterClassType type, byte level);

        /// <summary>
        ///     This method will search through algorithm service and return the JobLevelXp stat based on
        ///     <see cref="CharacterClassType" /> and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetJobLevelXp(CharacterClassType type, byte level);

        /// <summary>
        ///     This method will search through algorithm service and return the HeroLevelXp stat based on
        ///     <see cref="CharacterClassType" /> and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetHeroLevelXp(CharacterClassType type, byte level);

        /// <summary>
        ///     This method will search through algorithm service and return the SpLevelXp stat based on <see cref="SpType" /> and
        ///     level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetSpLevelXp(byte level);

        /// <summary>
        ///     This method will search through algorithm service and return the Fairy Level Xp stat based on the fairy's level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetFairyLevelXp(byte level);

        /// <summary>
        ///     This method will search through algorithm service and return the Fairy Level Xp stat based on the fairy's level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetFamilyLevelXp(byte level);

        #endregion


        #region Stats

        /// <summary>
        ///     This method will search through algorithm service and return the Speed stat based on
        ///     <see cref="CharacterClassType" /> and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetSpeed(CharacterClassType type, byte level);

        /// <summary>
        ///     This method will search through algorithm service and return the Close Defence stat based on
        ///     <see cref="CharacterClassType" /> and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetDefenceClose(CharacterClassType type, byte level);

        /// <summary>
        ///     This method will search through algorithm service and return the Ranged Defence stat based on
        ///     <see cref="CharacterClassType" /> and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetDefenceRange(CharacterClassType type, byte level);

        /// <summary>
        ///     This method will search through algorithm service and return the Magic Defence stat
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetDefenceMagic(CharacterClassType type, byte level);

        /// <summary>
        ///     This method will search through algorithm service and return the Dodge close stat based on
        ///     <see cref="CharacterClassType" /> and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetDodgeClose(CharacterClassType type, byte level);

        /// <summary>
        ///     This method will search through algorithm service and return the Dodge Ranged stat based on
        ///     <see cref="CharacterClassType" /> and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetDodgeRanged(CharacterClassType type, byte level);

        /// <summary>
        ///     This method will search through algorithm service and return the Dodge Magic stat based on
        ///     <see cref="CharacterClassType" /> and level
        ///     /!\ Should return the highest value under level if level is out of range
        ///     /!\/!\ Even if the base game logic tells magic attack does not miss, you can customise this :)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetDodgeMagic(CharacterClassType type, byte level);

        /// <summary>
        ///     This method will search through algorithm service and return the minimum attack range stat based on
        ///     <see cref="CharacterClassType" /> and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetMinimumAttackRange(CharacterClassType type, byte level);

        #endregion


        #region HpMp

        /// <summary>
        ///     Returns the maximum of Hp of a character, based on class and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetHpMax(CharacterClassType type, byte level);

        /// <summary>
        ///     Returns the maximum of Hp of a character, based on class and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetMpMax(CharacterClassType type, byte level);

        #region HpMpRegen

        /// <summary>
        ///     Returns the HpRegen when player is standing, based on class and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetHpRegen(CharacterClassType type, byte level);


        /// <summary>
        ///     Returns the HpRegen when player is sitting, based on class and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetHpRegenSitting(CharacterClassType type, byte level);

        /// <summary>
        ///     Returns the MpRegen when player is standing, based on class and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetMpRegen(CharacterClassType type, byte level);


        /// <summary>
        ///     Returns the HpRegen when player is sitting, based on class and level
        ///     /!\ Should return the highest value under level if level is out of range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetMpRegenSitting(CharacterClassType type, byte level);

        #endregion

        #endregion
    }
}