namespace ChickenAPI.Game.Permissions.Ranks
{
    public class OwnerPermissibleRank : IPermissibleRank
    {
        public string Name => "Owner";
        public bool HasPermission(PermissionType permission) => true;

        public bool HasPermission(string permission) => true;

        public void GrantPermission(PermissionType permission)
        {
        }

        public void GrantPermission(params PermissionType[] permission)
        {
        }

        public void GrantPermission(string permission)
        {
        }

        public void GrantPermission(params string[] permission)
        {
        }

        public void RevokePermission(PermissionType permission)
        {
        }

        public void RevokePermission(params PermissionType[] permissions)
        {
        }

        public void RevokePermission(string permission)
        {
        }

        public void RevokePermission(params string[] permissions)
        {
        }
    }
}