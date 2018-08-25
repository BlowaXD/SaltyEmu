namespace ChickenAPI.Enums.Game.Character
{
    public enum CharacterDignity : byte
    {
        Basic = 1, // Basic Dignity ( Dignity are not changed )
        Suspected = 2, // Suspected RANK  (Dignity: -100 ~ -200)
        BluffedNameOnly = 3, // Bluffed name only RANK (Dignity: -201 ~ -400)
        NotQualifiedFor = 4, // Not qualified for RANK (Dignity: -401 ~ -600)
        Useless = 5, // Useless RANK (Dignity: -601 ~ -800)
        StupidMinded = 6 // Stupid minded RANK (Dignity: -801 ~ -1.000)
    }
}