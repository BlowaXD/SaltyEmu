using System.Collections.Generic;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Families.Args
{
    public class FamilyCreateEventArgs : ChickenEventArgs
    {
        public IPlayerEntity Owner { get; set; }

        public string Name { get; set; }

        public IEnumerable<IPlayerEntity> Members { get; set; }

        /// <summary>
        /// Used to force family creation whether or not the player meets requirements
        /// </summary>
        public bool Force { get; set; }
    }
}