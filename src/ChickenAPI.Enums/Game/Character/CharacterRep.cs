namespace ChickenAPI.Enums.Game.Character
{
    public enum CharacterRep : sbyte
    {
        StupidMinded = -6, // Stupid minded RANK (Dignity: -801 ~ -1.000)
        Useless = -5, // Useless RANK (Dignity: -601 ~ -800)
        NotQualifiedFor = -4, // Not qualified for RANK (Dignity: -401 ~ -600)
        BluffedNameOnly = -3, // Bluffed name only RANK (Dignity: -201 ~ -400)
        Suspected = -2, // Suspected RANK  (Dignity: -100 ~ -200)
        Basic = -1, // Basic Dignity ( Dignity are not changed )
        Beginner = 1, //  (0 ~ 250 Reputation)

        IDk = 2, // IDK ?
        IDk2 = 3, // IDK ?

        TraineeG = 4, // (Reputation: 251 ~ 500)
        TraineeB = 5, //(Reputation:501 ~ 750)
        TraineeR = 6, //(Reputation: 750 ~ 1.000)

        TheExperiencedG = 7, // (Reputation: 1.001 ~ 2.250)
        TheExperiencedB = 8, // (Reputation: 2.251 ~ 3.500)
        TheExperiencedR = 9, // (Reputation: 3.501 ~ 5.000)

        BattleSoldierG = 10, // (Reputation: 5 001 ~ 9.500)
        BattleSoldierB = 11, // (Reputation: 9.501 ~ 19.000)
        BattleSoldierR = 12, // (Reputation: 19.001 ~ 25.000)

        ExpertG = 13, // (Reputation: 25.001 ~ 40.000)
        ExpertB = 14, // (Reputation: 40.001 ~ 60.000)
        ExpertR = 15, // (Reputation: 60.001 ~ 85.000)

        LeaderG = 16, // (Reputation: 85.001 ~ 115.000)
        LeaderB = 17, // (Reputation: 115.001 ~ 150.000)
        LeaderR = 18, // (Reputation: 150.001 ~ 190.000)

        MasterG = 19, // (Reputation: 190.001 ~ 235.000)
        MasterB = 20, // (Reputation: 235.001 ~ 185.000)
        MasterR = 21, // (Reputation: 285.001 ~ 350.000)

        NosG = 22, // (Reputation: 350.001 ~ 500.000)
        NosB = 23, // (Reputation: 500.001 ~ 1.500.000)
        NosR = 24, // (Reputation: 1.500.001 ~ 2.500.000)

        EliteG = 25, // (Reputation: 2.500.001 ~ 3.750.000)
        EliteB = 26, // (Reputation: 3.750.001 ~ 5.000.000)
        EliteR = 27, // (Reputation: 5.000.001 and more )

        LegendG = 28, // (43 th to 14 th place at the reputation ranking : 5.000.000 and more)
        LegendB = 29, // (14 th to 4 th place at the reputation ranking : 5.000.000 and more)

        AncienHeros = 30, // (3 rd place at the reputation ranking : 5.000.000 and more)
        MysteriousHeros = 31, // (2 nd place in the reputation ranking : 5.000.000 and more)
        LegendaryHeros = 32 // (1 st place in the reputation ranking : 5.000.000 and more)
    }
}