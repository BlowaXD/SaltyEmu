using System.Collections.Generic;

namespace ChickenAPI.Game.Permissions
{
    public abstract class PermissibleRankBase : IPermissibleRank
    {
        protected HashSet<string> PermissionsString = new HashSet<string>();
        protected HashSet<PermissionType> PermissionsTypes = new HashSet<PermissionType>();
        public abstract string Name { get; }

        public bool HasPermission(PermissionType permission)
        {
            return PermissionsTypes.Contains(permission);
        }

        public bool HasPermission(string permissionKey)
        {
            return PermissionsString.Contains(permissionKey);
        }

        public void GrantPermission(PermissionType permission)
        {
            PermissionsTypes.Add(permission);
        }

        public void GrantPermission(params PermissionType[] permissions)
        {
            foreach (PermissionType permission in permissions)
            {
                GrantPermission(permission);
            }
        }

        public void GrantPermission(string permissionKey)
        {
            PermissionsString.Add(permissionKey);
        }

        public void GrantPermission(params string[] permissions)
        {
            foreach (string permission in permissions)
            {
                GrantPermission(permission);
            }
        }

        public void RevokePermission(PermissionType permission)
        {
            PermissionsTypes.Remove(permission);
        }

        public void RevokePermission(params PermissionType[] permissions)
        {
            foreach (PermissionType permission in permissions)
            {
                RevokePermission(permission);
            }
        }

        public void RevokePermission(string permissionKey)
        {
            PermissionsString.Remove(permissionKey);
        }

        public void RevokePermission(params string[] permissions)
        {
            foreach (string permission in permissions)
            {
                RevokePermission(permission);
            }
        }
    }
}