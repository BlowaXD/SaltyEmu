namespace ChickenAPI.Game.Permissions
{
    public interface IRankService
    {
        IPermissibleRank GetPermissionByName(string name);
        T GetPermissionByName<T>(string name) where T : IPermissibleRank;
    }
}