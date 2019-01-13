using System.Collections.Generic;

namespace ChickenAPI.Game.Configuration.Item_Managements
{
    public class ProtectionItem : UpgradeItem
    {
        public Dictionary<ProtectionItemBenefit, float> Benefits { get; set; }
    }
}