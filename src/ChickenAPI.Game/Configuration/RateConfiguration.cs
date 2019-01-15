namespace ChickenAPI.Game.Configuration
{
    public struct RateConfiguration
    {
        /// <summary>
        ///     Includes :
        ///     Xp won on monsters
        /// </summary>
        public float GlobalXpMultiplier { get; set; }

        /// <summary>
        ///     Includes
        ///     Xp won on monsters
        /// </summary>
        public float GlobalJobXpMultiplier { get; set; }

        /// <summary>
        ///     Includes
        ///     Xp won on monsters
        /// </summary>
        public float GlobalSpXpMultiplier { get; set; }

        /// <summary>
        ///     Includes xp won on Act 6+ Monsters
        /// </summary>
        public float GlobalHeroXpMultiplier { get; set; }

        /// <summary>
        ///     Includes
        ///     Xp won on monsters (1 monster = 1 xp)
        /// </summary>
        public float GlobalFairyXpMultiplier { get; set; }

        /// <summary>
        /// </summary>
        public float GoldWinOnDropMultiplier { get; set; }

        /// <summary>
        ///     Includes
        ///     Xp won on monsters
        ///     Xp won from Medals
        /// </summary>
        public float GlobalReputationMultiplier { get; set; }
    }
}