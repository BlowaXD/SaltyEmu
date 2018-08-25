namespace ChickenAPI.Game.Permissions.Ranks
{
    public class OwnerRank : IRank
    {
        public string Name => "Owner";
        public bool HasPermission(PermissionType permission) => true;

        public bool HasPermission(string permission) => true;

        public void AddPermission(PermissionType permission)
        {
        }

        public void AddPermission(params PermissionType[] permission)
        {
        }

        public void AddPermission(string permission)
        {
        }

        public void AddPermission(params string[] permission)
        {
        }

        public void RemovePermission(PermissionType permission)
        {
        }

        public void RemovePermission(string permission)
        {
        }
    }
}