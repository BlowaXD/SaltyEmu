namespace ChickenAPI.Game
{
    public interface IGameConfiguration
    {
        /// <summary>
        /// Includes :
        /// Xp won on monsters
        /// </summary>
        float GlobalXpMultiplier { get; set; }

        /// <summary>
        /// Includes
        /// Xp won on monsters
        /// </summary>
        float GlobalJobXpMultiplier { get; set; }

        /// <summary>
        /// Includes
        /// Xp won on monsters
        /// </summary>
        float GlobalSpXpMultiplier { get; set; }

        /// <summary>
        /// Includes xp won on Act 6+ Monsters
        /// </summary>
        float GlobalHeroXpMultiplier { get; set; }

        /// <summary>
        /// Includes
        /// Xp won on monsters (1 monster = 1 xp)
        /// </summary>
        float GlobalFairyXpMultiplier { get; set; }

        /// <summary>
        /// 
        /// </summary>
        float GoldWinOnDropMultiplier { get; set; }

        /// <summary>
        /// Includes
        /// Xp won on monsters
        /// Xp won from Medals
        /// </summary>
        float GlobalReputationMultiplier { get; set; }

        /// <summary>
        /// The daily SP points you are supposed to have
        /// </summary>
        uint SpMaxDailyPoints { get; set; }

        /// <summary>
        /// The Sp Points you can store with cards or Sp Points potions
        /// </summary>
        uint SpMaxAdditionalPoints { get; set; }

        #region Gold

        float ItemPriceOnSellMultiplier { get; set; }
        float ItemPriceOnBuyMultiplier { get;set; }

        #endregion Gold
    }
}