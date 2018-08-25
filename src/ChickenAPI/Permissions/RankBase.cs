using System.Collections.Generic;

namespace ChickenAPI.Game.Permissions
{
    public abstract class RankBase : IRank
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

        public void AddPermission(PermissionType permission)
        {
            PermissionsTypes.Add(permission);
        }

        public void AddPermission(params PermissionType[] permissions)
        {
            foreach (PermissionType permission in permissions)
            {
                AddPermission(permission);
            }
        }

        public void AddPermission(string permissionKey)
        {
            PermissionsString.Add(permissionKey);
        }

        public void AddPermission(params string[] permissions)
        {
            foreach (string permission in permissions)
            {
                AddPermission(permission);
            }
        }

        public void RemovePermission(PermissionType permission)
        {
            PermissionsTypes.Remove(permission);
        }

        public void RemovePermission(string permissionKey)
        {
            PermissionsString.Remove(permissionKey);
        }
    }
}