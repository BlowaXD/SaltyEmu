using System.Collections.Generic;
using ChickenAPI.Enums.Game.Groups;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Groups
{
    public class Group
    {
        public long Id { get; set; }

        public IPlayerEntity Leader { get; set; }

        public List<IPlayerEntity> Players { get; set; }

        /// <summary>
        /// This is the drop OwnerShip type that is going to be attributed to every drops for a given group
        /// </summary>
        public GroupDropType DropType { get; set; }
    }
}