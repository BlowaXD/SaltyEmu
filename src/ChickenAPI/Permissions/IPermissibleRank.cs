namespace ChickenAPI.Game.Permissions
{
    public interface IPermissibleRank
    {
        /// <summary>
        /// PermissibleRank's name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Checks if the current rank has the given permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        bool HasPermission(PermissionType permission);

        bool HasPermission(string permission);

        /// <summary>
        /// Grants the permission to the actual rank
        /// </summary>
        /// <param name="permission"></param>
        void GrantPermission(PermissionType permission);

        void GrantPermission(params PermissionType[] permissions);
        void GrantPermission(string permission);
        void GrantPermission(params string[] permissions);

        /// <summary>
        /// Revokes the permission from the actual rank
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        void RevokePermission(PermissionType permission);

        void RevokePermission(params PermissionType[] permissions);

        void RevokePermission(string permission);
        void RevokePermission(params string[] permissions);
    }
}