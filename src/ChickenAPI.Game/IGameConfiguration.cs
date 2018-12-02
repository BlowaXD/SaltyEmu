namespace ChickenAPI.Game
{
    public interface IGameConfiguration
    {
        /// <summary>
        /// Includes :
        /// Xp won on monsters
        /// </summary>
        uint GlobalXpMultiplier { get; set; }

        /// <summary>
        /// Includes
        /// Xp won on monsters
        /// </summary>
        uint GlobalJobXpMultiplier { get; set; }

        /// <summary>
        /// Includes
        /// Xp won on monsters
        /// </summary>
        uint GlobalSpXpMultiplier { get;set; }

        /// <summary>
        /// Includes xp won on Act 6+ Monsters
        /// </summary>
        uint GlobalHeroXpMultiplier { get; set; }

        /// <summary>
        /// Includes
        /// Xp won on monsters (1 monster = 1 xp)
        /// </summary>
        uint GlobalFairyXpMultiplier { get; set; }

        /// <summary>
        /// 
        /// </summary>
        uint GoldWinOnDropMultiplier { get; set; }

        /// <summary>
        /// Includes
        /// Xp won on monsters
        /// Xp won from Medals
        /// </summary>
        uint GlobalReputationMultiplier { get; set; }
    }
}