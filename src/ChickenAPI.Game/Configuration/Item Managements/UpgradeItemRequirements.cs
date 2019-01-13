using System.Collections.Generic;

namespace ChickenAPI.Game.Configuration.Item_Managements
{
    public class UpgradeItemRequirements
    {
        /// <summary>
        ///     Upgrade rank (from +1 to +255)
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