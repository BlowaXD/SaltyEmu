namespace ChickenAPI.Game.Permissions
{
    public interface IRank
    {
        /// <summary>
        /// Rank's name
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
        /// Adds the permission to the rank
        /// </summary>
        /// <param name="permission"></param>
        void AddPermission(PermissionType permission);

        void AddPermission(params PermissionType[] permission);
        void AddPermission(string permission);
        void AddPermission(params string[] permission);

        /// <summary>
        /// Removes the permission from the Rank
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        void RemovePermission(PermissionType permission);

        void RemovePermission(string permission);
    }
}