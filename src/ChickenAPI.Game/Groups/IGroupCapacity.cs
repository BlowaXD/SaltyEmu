namespace ChickenAPI.Game.Groups
{
    public interface IGroupCapacity
    {
        /// <summary>
        ///     Group shared object
        /// </summary>
        Group Group { get; set; }

        /// <summary>
        ///     Is in group or not
        /// </summary>
        bool HasGroup { get; }

        /// <summary>
        ///     Tells if the player is the group Leader
        /// </summary>
        bool IsGroupLeader { get; }

        int GroupMembersCount { get; }
    }
}