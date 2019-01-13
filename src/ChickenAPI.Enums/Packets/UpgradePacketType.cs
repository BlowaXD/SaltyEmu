namespace ChickenAPI.Enums.Packets
{
    public enum UpgradePacketType
    {
        #region Item

        ItemToPart = 0,
        UpgradeItem = 1, //  UpgradeMode.Normal, UpgradeProtection.None, hasAmulet: HasAmulet
        CellonItem = 3,
        RarifyItem = 7, // mode, protection
        UpgradeItemProtected = 20, // UpgradeMode.Normal, UpgradeProtection.Protected, hasAmulet: HasAmulet
        RarifyItemProtected = 21, // RarifyMode.Normal, RarifyProtection.Scroll
        UpgradeItemGoldScroll = 43, // UpgradeMode.Reduced, UpgradeProtection.Protected, hasAmulet: hasAmulet ( Gold scroll)
        FusionItem = 53,

        #endregion Item

        SumResistance = 8,

        #region Specialist

        UpgradeSpNoProtection = 9, // UpgradeProtection.None
        UpgradeSpProtected = 25, // Scroll Blue or red - Need to verify
        UpgradeSpProtected2 = 26, // Scroll Blue or red - Need to verify
        PerfectSp = 41,
        UpgradeSpChiken = 35, // Scroll Chiken - event
        UpgradeSpPyjama = 38, // Scroll Pyjama - Event
        UpgradeSpPirate = 42, // Scroll Pirate - Event

        #endregion Specialist

        CreateFairyFernon = 52,
        CreateFairyErenia = 51,
        CreateFairyZenas = 50
    }
}