using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Groups;

namespace ChickenAPI.Game.Player
{
    public interface IGroupCapacity
    {
        /// <summary>
        /// Group shared object
        /// </summary>
        GroupDto Group { get; set; }

        /// <summary>
        /// Is in group or not
        /// </summary>
        bool HasGroup { get; }

        /// <summary>
        /// Tells if the player is the group Leader
        /// </summary>
        bool IsGroupLeader { get; }
    }

    public interface IGroupEntity : IGroupCapacity, IEntity
    {
    }
}