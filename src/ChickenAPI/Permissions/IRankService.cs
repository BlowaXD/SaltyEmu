namespace ChickenAPI.Game.Permissions
{
    public interface IRankService
    {
        IRank GetPermissionByName(string name);
        T GetPermissionByName<T>(string name) where T : IRank;
    }
}