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

        /// <summary>
        /// The daily SP points you are supposed to have
        /// </summary>
        uint SpMaxDailyPoints { get; set; }

        /// <summary>
        /// The Sp Points you can store with cards or Sp Points potions
        /// </summary>
        uint SpMaxAdditionalPoints { get; set; }
    }
}