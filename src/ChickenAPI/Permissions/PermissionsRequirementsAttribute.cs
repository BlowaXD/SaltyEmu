using System;

namespace ChickenAPI.Game.Permissions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class PermissionsRequirementsAttribute : Attribute
    {
        public PermissionsRequirementsAttribute(PermissionType type)
        {
            PermissionType = type;
        }

        public PermissionsRequirementsAttribute(string permissionKey)
        {
            PermissionName = permissionKey;
        }


        public string PermissionName { get; }
        public PermissionType PermissionType { get; }
    }
}