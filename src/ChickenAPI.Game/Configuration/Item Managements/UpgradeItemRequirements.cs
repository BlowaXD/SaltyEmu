using System.Collections.Generic;

namespace ChickenAPI.Game.Configuration.Item_Managements
{
    public class UpgradeItem
    {
        public long Id { get; set; }
        public long Quantity { get; set; }
    }

    public enum ProtectionItemBenefit
    {
        ItemNotDestroyedOnFail,
        ItemKeepUpgradeOnFail,
        SuccessGuaranteed,
        UpgradeChanceImproved,
        CostReduced
    }

    public class ProtectionItem : UpgradeItem
    {
        public Dictionary<ProtectionItemBenefit, float> Benefits { get; set; }
    }

    public class UpgradeItemRequirements
    {
        /// <summary>
        /// Upgrade rank (from +1 to +255)
        /// </summary>
        public byte UpgradeRank { get; set; }

        public List<UpgradeItem> ItemsNeeded { get; set; }

        public uint GoldAmount { get; set; }

        public List<ProtectionItem> Protection { get; set; }

        public float SuccessChance { get; set; }
        public float FailChance { get; set; }
        public float FixedChance { get; set; }
    }
}