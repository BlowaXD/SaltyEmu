namespace ChickenAPI.Enums.Game.Items
{
    public enum ArmorOptionType
    {
        /* ARMOR OPTIONS */

        // DEFENSE INCREASE
        CloseCombatDefense = 51,
        LongRangeDefense = 52,
        MagicalDefense = 53,
        SDefenseAllPercentage = 54,

        // ANTI-DEBUFFS
        ReducedMinorBleeding = 55,
        ReducedSeriousBleeding = 56,
        ReducedAllBleeding = 57,
        ReducedSmallBlackout = 58,
        ReducedAllBlackout = 59,
        ReducedHandOfDeath = 60,
        ReducedFrozenChance = 61,
        ReducedBlindChance = 62,
        ReducedArrestationChance = 63,
        ReducedDefenseReduction = 64,
        ReducedShockChance = 65,
        ReducedRigidityChance = 66,
        SReducedAllNegative = 67,

        // CHARACTER BONUSES
        OnRestHpRecoveryPercentage = 68,
        NaturalHpRecoveryPercentage = 69,
        OnRestMpRecoveryPercentage = 70,
        NaturalMpRecoveryPercentage = 71,
        SOnAttackRecoveryPercentage = 72,
        ReduceCriticalChance = 73,

        // RESISTANCE INCREASE
        FireResistanceIncrease = 74,
        WaterResistanceIncrease = 75,
        LightResistanceIncrease = 76,
        DarkResistanceIncrease = 77,
        SIncreaseAllResistance = 78,

        // VARIOUS PVE BONUSES
        DignityLossReduced = 79,
        PointConsumptionReduced = 80,
        MiniGameProductionIncreased = 81,
        FoodHealing = 82,

        // PVP BONUSES
        PvpDefensePercentage = 83,
        PvpDodgeClose = 84,
        PvpDodgeRanged = 85,
        PvpDodgeMagic = 86,
        SPvpDodgeAll = 87,
        PvpMpProtect = 88,

        // R8 OPTIONS
        ChampionPvpIgnoreAttackFire = 89,
        ChampionPvpIgnoreAttackWater = 90,
        ChampionPvpIgnoreAttackLight = 91,
        ChampionPvpIgnoreAttackDark = 92,
        AbsorbDamagePercentageA = 93,
        AbsorbDamagePercentageB = 94,
        AbsorbDamagePercentageC = 95,
        IncreaseEvasiveness = 96
    }
}