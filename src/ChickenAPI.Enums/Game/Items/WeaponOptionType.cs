namespace ChickenAPI.Enums.Game.Items
{
    public enum WeaponOptionType : byte
    {
        /* WEAPON OPTIONS */
        // INCREASE DAMAGES
        IncreaseDamage = 1,
        SDamagePercentage = 2,

        // DEBUFFS
        MinorBleeding = 3,
        Bleeding = 4,
        SeriousBleeding = 5,
        Blackout = 6,
        Frozen = 7,
        DeadlyBlackout = 8,

        // PVE OPTIONS
        IncreaseDamageOnPlants = 9,
        IncreaseDamageOnAnimals = 10,
        IncreaseDamageOnDemons = 11,
        IncreaseDamagesOnZombies = 12,
        IncreaseDamagesOnSmallAnimals = 13,
        SDamagePercentageOnGiantMonsters = 14,

        // CHARACTER BONUSES
        IncreaseCritChance = 15,
        IncreaseCritDamages = 16,
        ProtectWandSkillInterruption = 17,
        IncreaseFireElement = 18,
        IncreaseWaterElement = 19,
        IncreaseLightElement = 20,
        IncreaseDarknessElement = 21,
        SIncreaseAllElements = 22,
        ReduceMpConsumption = 23,
        HpRegenerationOnKill = 24,
        MpRegenerationOnKill = 25,

        // SP BONUSES
        AttackSl = 26,
        DefenseSl = 27,
        ElementSl = 28,
        HpMpSl = 29,
        SGlobalSl = 30,

        // PVE RATES INCREASE
        GoldPercentage = 31,
        XpPercentage = 32,
        JobXpPercentage = 33,

        // PVP OPTIONS
        PvpDamagePercentage = 34,
        PvpEnemyDefenseDecreased = 35,
        PvpResistanceDecreasedFire = 36,
        PvpResistanceDecreasedWater = 37,
        PvpResistanceDecreasedLight = 38,
        PvpResistanceDecreasedDark = 39,
        PvpResistanceDecreasedAll = 40,
        PvpAlwaysHit = 41,
        PvpDamageProbabilityPercentage = 42,
        PvpWithdrawMp = 43,

        // R8 CHAMPION OPTIONS
        PvpIgnoreResistanceFire = 44,
        PvpIgnoreResistanceWater = 45,
        PvpIgnoreResistanceLight = 46,
        PvpIgnoreResistanceDark = 47,
        RegenSpecialistPointPerKill = 48,
        IncreasePrecision = 49,
        IncreaseConcentration = 50
    }
}