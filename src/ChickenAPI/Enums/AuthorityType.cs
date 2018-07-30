namespace ChickenAPI.Enums
{
    /// <summary>
    ///     Account basic groups
    /// </summary>
    public enum AuthorityType : short
    {
        Banned = -100,
        Muted = -10,
        WaitingForValidation = -5,
        User = 0,
        Support = 80,
        GameMaster = 100,
        Administrator = 1000
    }
}