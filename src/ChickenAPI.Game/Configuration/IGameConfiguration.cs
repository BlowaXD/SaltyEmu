using ChickenAPI.Game.Configuration.Item_Managements;

namespace ChickenAPI.Game.Configuration
{
    public interface IGameConfiguration
    {
        RateConfiguration Rates { get; set; }

        InventoryConfiguration Inventory { get; set; }

        /// <summary>
        ///     The daily SP points you are supposed to have
        /// </summary>
        uint SpMaxDailyPoints { get; set; }

        /// <summary>
        ///     The Sp Points you can store with cards or Sp Points potions
        /// </summary>
        uint SpMaxAdditionalPoints { get; set; }

        #region UpgradeConfiguration

        RarifyChancesConfiguration RarifyChances { get; set; }
        UpgradeConfigurationItem UpgradeItem { get; set; }

        UpgradeConfigurationSp UpgradeSp { get; set; }
        SummingConfiguration Summing { get; set; }
        PerfectSpConfiguration PerfectSp { get; set; }

        #endregion

        #region Gold

        float ItemPriceOnSellMultiplier { get; set; }
        float ItemPriceOnBuyMultiplier { get; set; }

        uint MaxGold { get; set; }

        // <Total>:
        string GlobalChatPrefix { get; set; }

        #endregion Gold
    }
}